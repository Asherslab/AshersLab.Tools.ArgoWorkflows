using System.Text;
using AshersLab.Tools.ArgoWorkflows.Configuration;
using AshersLab.Tools.ArgoWorkflows.Interfaces;
using AshersLab.Tools.ArgoWorkflows.Models.BuildSteps;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Builders.Workflows;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Builders.Workflows.Templates.DAG;

namespace AshersLab.Tools.ArgoWorkflows.Services.BuildSteps;

public class DotnetBuildStepService : IBuildStepService
{
    public const string Name = "dotnet publish";

    private readonly RunConfig               _runConfig;
    private readonly SolutionExplorerService _solutionExplorer;

    public DotnetBuildStepService(RunConfig runConfig, SolutionExplorerService solutionExplorer)
    {
        _runConfig = runConfig;
        _solutionExplorer = solutionExplorer;
    }

    public void ConfigureWorkflow(WorkflowBuilder workflowBuilder)
    {
        IEnumerable<IProject> projects = _solutionExplorer
            .Explore(_runConfig.SolutionFile)
            .Where(x => x.BuildSteps.Any(y => y is DotnetBuildStep));
        foreach (IProject project in projects)
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

            StringBuilder imageBuilder =
                new($"mcr.microsoft.com/dotnet/sdk:{project.TargetFramework.Replace("net", "").Replace("coreapp", "").Replace("standard2.1", "6.0")}");

            if (_runConfig.Images?.DotnetSdk != null)
            {
                imageBuilder = new StringBuilder();
                imageBuilder.Append(_runConfig.Images.DotnetSdk);
                if (!imageBuilder.ToString().Contains(':'))
                    imageBuilder.Append(':').Append(project.TargetFramework.Replace("net", "").Replace("coreapp", "").Replace("standard2.1", "6.0"));
            }
            
            // @formatter:off
            workflowBuilder
                .AddContainerTemplate()
                .SetName($"{Name} {project.Name}")
                .SetContainer()
                    .SetImage(imageBuilder.ToString())
                    .SetCommand("sh", "-c")
                    .AddArgument(
                        nugetSourcesBuilder +
                        $"dotnet restore --packages {_runConfig.PersistenceVolumePath}/nuget {_runConfig.PersistenceVolumePath}/src/{project.RelativeLocation} && " +
                        $"dotnet publish --no-restore --no-dependencies -c Release -o {_runConfig.PersistenceVolumePath}/publish/{project.Name} {_runConfig.PersistenceVolumePath}/src/{project.RelativeLocation} && " +
                        
                        // caching stuff ----
                        // csproj
                        $"mkdir -p /new/{project.Name}/ && " +
                        $"cp -r {_runConfig.PersistenceVolumePath}/src/{project.RelativeLocation} {project.Name}/ && " +
                        "echo 1 && " +
                        
                        // obj
                        $"mkdir -p /new/{project.Name}/obj/ && " +
                        $"cp -r {_runConfig.PersistenceVolumePath}/src/{project.RelativeDirectory}/obj/project.assets.json {project.Name}/obj/ && " +
                        "echo 2 && " +
                        $"cp -r {_runConfig.PersistenceVolumePath}/src/{project.RelativeDirectory}/obj/*.csproj.nuget.* {project.Name}/obj/ && " +
                        "echo 3 && " +
                        
                        // bin
                        $"mkdir -p /new/{project.Name}/bin/Release/{project.TargetFramework}/ && " +
                        $"cp -r {_runConfig.PersistenceVolumePath}/publish/{project.Name}/* {project.Name}/bin/Release/{project.TargetFramework}/ && " +
                        "echo 4 && " +
                        
                        // remove old
                        $"rm -rf {_runConfig.PersistenceVolumePath}/src/{project.RelativeDirectory} && " +
                        "echo 5 && " +
                        
                        // move in new
                        $"mv /new/{project.Name} {_runConfig.PersistenceVolumePath}/src/{project.RelativeDirectory} && " +
                        $"ls {_runConfig.PersistenceVolumePath}/src/{project.RelativeDirectory}"
                    )
                    .AddVolumeMount("persistence", _runConfig.PersistenceVolumePath);
            // @formatter:on
        }
    }

    public void ConfigureDAG(DAGSpecBuilder dagSpecBuilder)
    {
        ICollection<IProject> projects = _solutionExplorer
            .Explore(_runConfig.SolutionFile)
            .Where(x => x.BuildSteps.Any(y => y is DotnetBuildStep))
            .ToList();
        foreach (IProject project in projects)
        {
            // @formatter:off
            DAGTaskBuilder dagTaskBuilder = dagSpecBuilder
                .AddTask()
                    .SetName($"{Name} {project.Name}")
                    .SetTemplate($"{Name} {project.Name}");
            // @formatter:on

            ICollection<IProject> dependencies = project.GetDependencies(projects).ToList();

            if (!dependencies.Any())
            {
                dagTaskBuilder.AddDependency(CheckoutBuildStepService.Name);
                continue;
            }

            foreach (IProject dependency in dependencies)
            {
                dagTaskBuilder.AddDependency($"{Name} {dependency.Name}");
            }
        }
    }
}