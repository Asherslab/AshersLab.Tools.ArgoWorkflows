using System.Text;
using AshersLab.Tools.ArgoWorkflows.Configuration;
using AshersLab.Tools.ArgoWorkflows.Interfaces;
using AshersLab.Tools.ArgoWorkflows.Models.BuildSteps;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Builders.Workflows;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Builders.Workflows.Templates.DAG;

namespace AshersLab.Tools.ArgoWorkflows.Services.BuildSteps;

public class NugetStepService : IBuildStepService
{
    public const string Name = "nuget";

    private readonly RunConfig               _runConfig;
    private readonly SolutionExplorerService _solutionExplorer;

    public NugetStepService(RunConfig runConfig, SolutionExplorerService solutionExplorer)
    {
        _runConfig = runConfig;
        _solutionExplorer = solutionExplorer;
    }

    public void ConfigureWorkflow(WorkflowBuilder workflowBuilder)
    {
        IEnumerable<IProject> projects = _solutionExplorer
            .Explore(_runConfig.SolutionFile)
            .Where(x => x.BuildSteps.Any(y => y is NugetStep));
        foreach (IProject project in projects)
        {
            StringBuilder nugetSourcesBuilder = new();
            if (_runConfig.NugetSources != null && _runConfig.NugetSources.Any())
            {
                nugetSourcesBuilder.Append("dotnet nuget remove source nuget.org\n");
                foreach (string source in _runConfig.NugetSources)
                {
                    nugetSourcesBuilder.Append($"dotnet nuget add source {source}\n");
                }
            }

            StringBuilder imageBuilder = new($"mcr.microsoft.com/dotnet/sdk:{project.TargetFramework.Replace("net", "").Replace("coreapp", "").Replace("standard2.1", "6.0")}");

            if (_runConfig.Images?.DotnetSdk != null)
            {
                imageBuilder = new StringBuilder();
                imageBuilder.Append(_runConfig.Images.DotnetSdk);
                if (!imageBuilder.ToString().Contains(':'))
                    imageBuilder.Append(':').Append(project.TargetFramework.Replace("net", "").Replace("coreapp", "").Replace("standard2.1", "6.0"));
            }

            // @formatter:off
            workflowBuilder
                .AddScriptTemplate()
                .SetName($"{Name} {project.Name}")
                .SetScript()
                    .SetSource(
                        nugetSourcesBuilder +
                        $"curl -L {_runConfig.JqToolUrl} > /usr/bin/jq\n" +
                        $"curl -L {_runConfig.SemverToolUrl} > /usr/bin/semver\n" +
                        "chmod +x /usr/bin/semver && chmod +x /usr/bin/jq\n" +
                        $"current_version=$(curl {_runConfig.NugetPushUrl}/registration/{project.Name}/index.json -s | jq -r \".items[0].upper\")\n" +
                        "[ -z $current_version ] && next_version=1.0.0 || next_version=$(semver bump patch $current_version) || next_version=1.0.0\n" +
                        "echo $current_version && echo $next_version\n" +
                        $"dotnet pack --no-build {_runConfig.PersistenceVolumePath}/src/{project.RelativeLocation} --include-source --configuration Release --output . /p:Version=\"${{next_version}}\"\n" +
                        "dotnet nuget push "+
                            $"\"{project.Name}.${{next_version}}\".nupkg "+
                            "--api-key \"$NUGET_API_KEY\" " +
                            $"--source {_runConfig.NugetPushUrl}/index.json"
                    )
                    .SetImage(imageBuilder.ToString())
                    .SetCommand("sh")
                    .AddEnv()
                        .SetName("NUGET_API_KEY")
                        .SetExternalValue()
                            .SetSecret()
                                .SetName("nuget-secret")
                                .SetKey("api-key")
                                .Up()
                            .Up()
                        .Up()
                    .AddVolumeMount("persistence", _runConfig.PersistenceVolumePath);
            // @formatter:on
        }
    }

    public void ConfigureDAG(DAGSpecBuilder dagSpecBuilder)
    {
        IEnumerable<IProject> projects = _solutionExplorer
            .Explore(_runConfig.SolutionFile)
            .Where(x => x.BuildSteps.Any(y => y is NugetStep));
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