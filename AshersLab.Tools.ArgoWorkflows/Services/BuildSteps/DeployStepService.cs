using System.Text;
using AshersLab.Tools.ArgoWorkflows.Configuration;
using AshersLab.Tools.ArgoWorkflows.Interfaces;
using AshersLab.Tools.ArgoWorkflows.Models.BuildSteps;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Builders.Workflows;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Builders.Workflows.Templates.DAG;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Builders.Workflows.Templates.Scripts;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Extensions;

namespace AshersLab.Tools.ArgoWorkflows.Services.BuildSteps;

public class DeployStepService : IBuildStepService
{
    public const string Name = "deploy";

    private readonly RunConfig               _runConfig;
    private readonly SolutionExplorerService _solutionExplorer;

    public DeployStepService(RunConfig runConfig, SolutionExplorerService solutionExplorer)
    {
        _runConfig = runConfig;
        _solutionExplorer = solutionExplorer;
    }

    public void ConfigureWorkflow(WorkflowBuilder workflowBuilder)
    {
        ICollection<DeployDockerStep> projects = _solutionExplorer
            .Explore(_runConfig.SolutionFile)
            .Where(x => x.BuildSteps.Any(y => y is DeployDockerStep))
            .Select(x => (x.BuildSteps.Find(y => y is DeployDockerStep) as DeployDockerStep)!)
            .ToList();

        foreach (IGrouping<string, DeployDockerStep> grouping in projects.GroupBy(x =>
                     x.DeploymentRepo))
        {
            string repo = grouping.Key;

            // @formatter:off
            ScriptSpecBuilder scriptBuilder = workflowBuilder
                .AddScriptTemplate()
                .SetName($"{Name} {repo.Split("/", 4).Last().Replace(".git", "").ArgoNormalize()}")
                .SetInputs()
                    .AddGitArtifact()
                        .SetName("source")
                        .SetPath("/deploy")
                        .SetSpec()
                            .SetIgnoreHostKey()
                            .SetRepository(repo)
                            .SetSshPrivateKeySecret("gitea-secret", "ssh-privatekey")
                            .Up()
                        .Up()
                    .Up()
                .AddVolume()
                    .SetName("gitea-key")
                    .SetSecret("gitea-secret")
                    .Up()
                .SetScript();
            // @formatter:on

            StringBuilder stringBuilder = new();

            stringBuilder.Append("apk add git openssh-client\n");
            stringBuilder.Append("mkdir ~/.ssh\n");
            stringBuilder.Append("cp /mnt/gitea/ssh-privatekey ~/.ssh/id_rsa\n");
            stringBuilder.Append("chmod 0600 ~/.ssh/id_rsa\n");

            foreach (IGrouping<string?, DeployDockerStep> steps in grouping.GroupBy(x =>
                         x.DeploymentRepoDirectory)
                    )
            {
                if (steps.Key != null)
                    stringBuilder.Append($"cd {steps.Key}\n");

                foreach (DeployDockerStep step in steps)
                {
                    stringBuilder.Append(
                        $"kustomize edit set image {step.DeploymentImageReplacement}={step.DeploymentImage}\n");
                }

                if (steps.Key != null)
                {
                    stringBuilder.Append("cd -\n");
                    stringBuilder.Append($"git add {steps.Key}/kustomization.yaml\n");
                }
                else
                {
                    stringBuilder.Append("git add kustomization.yaml\n");
                }
            }

            stringBuilder.Append($"git config user.email \"{_runConfig.GitDeployCommitEmail}\"\n");
            stringBuilder.Append("git commit -m \"Update to {{workflow.parameters.hash}}\"\n");
            stringBuilder.Append(
                "GIT_SSH_COMMAND=\"ssh -o UserKnownHostsFile=/dev/null -o StrictHostKeyChecking=no\" git push\n");

            scriptBuilder
                .SetSource(stringBuilder.ToString())
                .SetImage(_runConfig.Images?.Kustomize ?? "traherom/kustomize-docker:2.0.0")
                .SetCommand("sh")
                .SetWorkingDirectory("/deploy")
                .AddVolumeMount("gitea-key", "/mnt/gitea");
        }
    }

    public void ConfigureDAG(DAGSpecBuilder dagSpecBuilder)
    {
        ICollection<Tuple<IProject, DeployDockerStep>> projects = _solutionExplorer
            .Explore(_runConfig.SolutionFile)
            .Where(x => x.BuildSteps.Any(y => y is DeployDockerStep))
            .Select(x =>
                new Tuple<IProject, DeployDockerStep>(
                    x,
                    (x.BuildSteps.Find(y => y is DeployDockerStep) as DeployDockerStep)!
                )
            )
            .ToList();

        foreach (IGrouping<string, Tuple<IProject, DeployDockerStep>> grouping in projects.GroupBy(x =>
                     x.Item2.DeploymentRepo))
        {
            string repo = grouping.Key;

            // @formatter:off
            DAGTaskBuilder dagTaskBuilder = dagSpecBuilder
                .AddTask()
                    .SetName($"{Name} {repo.Split("/", 4).Last().Replace(".git", "").ArgoNormalize()}")
                    .SetTemplate($"{Name} {repo.Split("/", 4).Last().Replace(".git", "").ArgoNormalize()}");
            // @formatter:on

            foreach (Tuple<IProject, DeployDockerStep> step in grouping)
            {
                dagTaskBuilder.AddDependency($"{DockerBuildStepService.Name} {step.Item1.Name}");
            }
        }
    }
}