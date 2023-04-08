using System.Text;
using AshersLab.Tools.ArgoWorkflows.Configuration;
using AshersLab.Tools.ArgoWorkflows.Interfaces;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Builders.Workflows;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Builders.Workflows.Templates.DAG;

namespace AshersLab.Tools.ArgoWorkflows.Services.BuildSteps;

public class RestoreBuildStepService : IBuildStepService
{
    public static string Name = "restore";

    private readonly RunConfig _runConfig;

    public RestoreBuildStepService(RunConfig runConfig)
    {
        _runConfig = runConfig;
    }

    public void ConfigureWorkflow(WorkflowBuilder workflowBuilder)
    {
        StringBuilder nugetSourcesBuilder = new();
        if (_runConfig.NugetSources != null && _runConfig.NugetSources.Any())
        {
            nugetSourcesBuilder.Append("dotnet nuget remove source nuget.org &&");
            foreach (string source in _runConfig.NugetSources)
            {
                nugetSourcesBuilder.Append($"dotnet nuget add source {source} && ");
            }
        }
        
        // @formatter:off
        workflowBuilder.AddContainerTemplate()
            .SetName(Name)
            .SetContainer()
                .SetImage("mcr.microsoft.com/dotnet/sdk:7.0")
                .SetCommand("sh", "-c")
                .AddArgument(
                    nugetSourcesBuilder +
                    $"dotnet restore --packages {_runConfig.PersistenceVolumePath}/nuget {_runConfig.PersistenceVolumePath}/src/"
                )
                .AddVolumeMount("persistence", _runConfig.PersistenceVolumePath);
        // @formatter:on
    }

    public void ConfigureDAG(DAGSpecBuilder dagSpecBuilder)
    {
        // @formatter:off
        dagSpecBuilder
            .AddTask()
                .SetName(Name)
                .SetTemplate(Name)
                .AddDependency(CheckoutBuildStepService.Name);
        // @formatter:on
    }
}