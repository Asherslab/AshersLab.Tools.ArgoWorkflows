using AshersLab.Tools.ArgoWorkflows.Configuration;
using AshersLab.Tools.ArgoWorkflows.Interfaces;
using AshersLab.Tools.ArgoWorkflows.Models.BuildSteps;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Builders.Workflows;
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

            // @formatter:off
            workflowBuilder
                .AddContainerTemplate()
                .SetName($"{Name} {project.Name}")
                .AddVolume()
                    .SetName("docker-config")
                    .SetSecret("docker-config")
                    .Up()
                .SetContainer()
                    .SetImage(_runConfig.Images?.BuildKit ?? "moby/buildlkit:v0.9.1")
                    .SetCommand("buildctl-daemonless.sh")
                    .AddArgument("build")
                    .AddArgument("--frontend")
                    .AddArgument("dockerfile.v0")
                    .AddArgument("--local")
                    .AddArgument($"context={_runConfig.PersistenceVolumePath}")
                    .AddArgument("--local")
                    .AddArgument($"dockerfile={_runConfig.PersistenceVolumePath}/src/{project.RelativeDirectory}")
                    .AddArgument("--opt")
                    .AddArgument($"filename={dockerBuildStep.DockerFile}")
                    .AddArgument("--opt")
                    .AddArgument($"build-arg:publish=./publish/{project.Name}")
                    .AddArgument("--opt")
                    .AddArgument($"build-arg:project=./src/{project.RelativeDirectory}")
                    .AddArgument("--output")
                    .AddArgument($"type=image,\"name={dockerBuildStep.DockerImages.Aggregate((prev, next) => $"{prev},{next}")}\",push=true")
                    .AddVolumeMount("persistence", _runConfig.PersistenceVolumePath)
                    .AddVolumeMount("docker-config", "/.docker")
                    .AddEnv("DOCKER_CONFIG", "/.docker")
                    .AddSecurityContext()
                        .SetPrivileged();
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