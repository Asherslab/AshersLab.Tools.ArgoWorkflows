// See https://aka.ms/new-console-template for more information

using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AshersLab.Tools.ArgoWorkflows.Configuration;
using AshersLab.Tools.ArgoWorkflows.Extensions;
using AshersLab.Tools.ArgoWorkflows.Interfaces;
using AshersLab.Tools.ArgoWorkflows.Interfaces.Services;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Builders;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Builders.Workflows;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Models;
using AshersLab.Tools.ArgoWorkflows.Services;
using AshersLab.Tools.ArgoWorkflows.Services.BuildSteps;
using AshersLab.Tools.ArgoWorkflows.Utilities;

IHostBuilder builder = Host.CreateDefaultBuilder();

builder.AddConfigService<RunConfig>("config", x =>
{
    if (x.SolutionFile == null!)
    {
        x.SolutionFile = Directory.EnumerateFiles(x.RepoDirectory).Single(y => y.EndsWith(".sln"));
    }
});
builder.ConfigureServices(x =>
{
    x
        .AddTransient<SolutionExplorerService>()
        .AddTransient<IProjectInSolutionHandler, ProjectInSolutionHandler>()
        .AddTransient<IEntrypointService, DAGEntrypointService>()
        .AddTransient<IBuildStepService, CheckoutBuildStepService>()
        .AddTransient<IBuildStepService, DotnetBuildStepService>()
        .AddTransient<IBuildStepService, DockerBuildStepService>()
        .AddTransient<IBuildStepService, NugetStepService>()
        .AddTransient<IBuildStepService, DeployStepService>();
});

IHost host = builder.Build();

MSBuildUtil.SetMsBuildPath();
RunConfig runConfig = host.Services.GetRequiredService<RunConfig>();

// i want this indented nicely!
// @formatter:off
// setup resource
KubernetesResourceBuilder resourceBuilder = new KubernetesResourceBuilder()
    .SetMetadata()
        .SetNamespace("argo-workflows-build")
        .SetGenerateName("build-dotnet-workflow-")
        .Up();

// setup workflow
WorkflowBuilder workflowBuilder = resourceBuilder.SetAsWorkflow()
    .SetEntrypoint("execute")
    .SetParallelism(runConfig.MaxParallelism)
    .AddArgumentsParameter("hash", runConfig.TargetHash ?? "")
    .AddArgumentsParameter("src_repo", runConfig.SourceRepo ?? "")
    .AddWorkflowVolume()
        .SetMetadata()
            .SetName("persistence")
            .Up()
        .SetVolumeClaim()
            .SetStorageClassName(runConfig.StorageClassName)
            .AddAccessMode(runConfig.VolumeAccessMode)
            .SetResources(runConfig.PersistentVolumeSize, StorageSizes.Mi)
            .Up()
        .Up();
// @formatter:on

foreach (IBuildStepService buildStepService in host.Services.GetServices<IBuildStepService>())
{
    buildStepService.ConfigureWorkflow(workflowBuilder);
}

IEntrypointService entrypointService = host.Services.GetRequiredService<IEntrypointService>();

entrypointService.ConfigureWorkflow(workflowBuilder);

if (runConfig.ToolOutput != null)
{
    await File.WriteAllTextAsync(runConfig.ToolOutput, JsonSerializer.Serialize(resourceBuilder.Build(),
        new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        }));
}
else
{
    Console.WriteLine(JsonSerializer.Serialize(resourceBuilder.Build(), new JsonSerializerOptions
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    }));
}