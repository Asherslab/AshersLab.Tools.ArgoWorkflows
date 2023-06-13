using System.Text;
using AshersLab.Tools.ArgoWorkflows.Configuration;
using AshersLab.Tools.ArgoWorkflows.Interfaces;
using AshersLab.Tools.ArgoWorkflows.Models.BuildSteps;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Builders.Workflows;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Builders.Workflows.Templates.Containers;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Builders.Workflows.Templates.DAG;

namespace AshersLab.Tools.ArgoWorkflows.Services.BuildSteps;

public class DockerBuildStepService : IBuildStepService
{
    public const string Name = "docker publish";

    private readonly RunConfig               _runConfig;
    private readonly SolutionExplorerService _solutionExplorer;

    public DockerBuildStepService(RunConfig runConfig, SolutionExplorerService solutionExplorer)
    {
        _runConfig = runConfig;
        _solutionExplorer = solutionExplorer;
    }

    public void ConfigureWorkflow(WorkflowBuilder workflowBuilder)
    {
        IEnumerable<IProject> projects = _solutionExplorer
            .Explore(_runConfig.SolutionFile)
            .Where(x => x.BuildSteps.Any(y => y is DockerBuildStep));
        foreach (IProject project in projects)
        {
            IBuildStep? buildStep = project.BuildSteps.Find(x => x is DockerBuildStep);
            if (buildStep is not DockerBuildStep dockerBuildStep)
                continue;

            string platformArgument = dockerBuildStep.Platforms == null
                ? ""
                : $"--opt platform={dockerBuildStep.Platforms.Aggregate((prev, next) => $"{prev},{next}")} ";

            StringBuilder scriptBuilder = new();

            if (_runConfig.InstallECRLoginTools)
            {
                scriptBuilder.Append("apk add docker-credential-ecr-login && ");
            }

            StringBuilder imageNames = new();

            foreach (string dockerImage in dockerBuildStep.DockerImages)
            {
                imageNames.Append($"name={dockerImage},");
            }

            scriptBuilder
                .Append("buildctl-daemonless.sh ")
                .Append("build ")
                .Append("--frontend dockerfile.v0 ")
                .Append(platformArgument)
                .Append($"--local context={_runConfig.PersistenceVolumePath} ")
                .Append($"--local dockerfile={_runConfig.PersistenceVolumePath}/src/{project.RelativeDirectory} ")
                .Append($"--opt filename={dockerBuildStep.DockerFile} ")
                .Append($"--opt build-arg:publish=./publish/{project.Name} ")
                .Append($"--opt build-arg:project=./src/{project.RelativeDirectory} ")
                .Append(
                    $"--output type=image,{imageNames}push=true"
                );

            // @formatter:off
            ContainerTemplateBuilder<WorkflowBuilder> containerTemplateBuilder = workflowBuilder
                .AddContainerTemplate()
                    .SetName($"{Name} {project.Name}")
                    .AddVolume()
                        .SetName("docker-config")
                        .SetSecret("docker-config")
                        .Up();
            
            if (_runConfig.ECRServiceAccountName != null)
            {
                containerTemplateBuilder.SetServiceAccountName(_runConfig.ECRServiceAccountName);
            }
            
            ContainerBuilder<ContainerTemplateBuilder<WorkflowBuilder>> containerBuilder = containerTemplateBuilder
                    .SetContainer();
            
            if (_runConfig.ECRRegistryEnvironmentLogin)
            {
                containerBuilder
                    .AddEnv()
                        .SetName("AWS_ACCESS_KEY_ID")
                        .SetExternalValue()
                            .SetSecret()
                                .SetName("aws-ecr-credentials")
                                .SetKey("key-id");
                
                containerBuilder
                    .AddEnv()
                        .SetName("AWS_SECRET_ACCESS_KEY")
                        .SetExternalValue()
                            .SetSecret()
                                .SetName("aws-ecr-credentials")
                                .SetKey("secret");
            }
            
            containerBuilder
                .SetImage(_runConfig.Images?.BuildKit ?? "moby/buildkit:v0.9.1")
                .AddResources()
                    .SetLimits(_runConfig.DockerBuildMemory, _runConfig.DockerBuildCpu)
                    .SetRequests(_runConfig.DockerBuildMemory, _runConfig.DockerBuildCpu)
                    .Up()
                .SetCommand("sh", "-c")
                    .AddArgument(scriptBuilder.ToString())
                .AddVolumeMount("persistence", _runConfig.PersistenceVolumePath)
                .AddVolumeMount("docker-config", "/.docker")
                .AddEnv("DOCKER_CONFIG", "/.docker")
                .AddEnv("BUILDCTL_CONNECT_RETRIES_MAX", "100")
                .AddSecurityContext()
                    .SetPrivileged()
                    .SetSeLinuxType(_runConfig.DockerBuildSeLinuxType);
            // @formatter:on
        }
    }

    public void ConfigureDAG(DAGSpecBuilder dagSpecBuilder)
    {
        IEnumerable<IProject> projects = _solutionExplorer
            .Explore(_runConfig.SolutionFile)
            .Where(x => x.BuildSteps.Any(y => y is DockerBuildStep));
        foreach (IProject project in projects)
        {
            // @formatter:off
            dagSpecBuilder
                .AddTask()
                    .SetName($"{Name} {project.Name}")
                    .SetTemplate($"{Name} {project.Name}")
                    .AddDependency($"{DotnetBuildStepService.Name} {project.Name}");
            // @formatter:on
        }
    }
}