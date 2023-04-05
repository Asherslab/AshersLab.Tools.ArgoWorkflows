using AshersLab.Tools.ArgoWorkflows.Configuration;
using AshersLab.Tools.ArgoWorkflows.Interfaces;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Builders.Workflows;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Builders.Workflows.Templates.DAG;

namespace AshersLab.Tools.ArgoWorkflows.Services.BuildSteps;

public class CheckoutBuildStepService : IBuildStepService
{
    public static string Name = "checkout";

    private readonly RunConfig _runConfig;

    public CheckoutBuildStepService(RunConfig runConfig)
    {
        _runConfig = runConfig;
    }

    public void ConfigureWorkflow(WorkflowBuilder workflowBuilder)
    {
        // @formatter:off
        workflowBuilder.AddContainerTemplate()
            .SetName(Name)
            .SetInputs()
                .AddGitArtifact()
                    .SetName("source")
                    .SetPath("/mnt/persistence/src")
                    .SetSpec()
                        .SetIgnoreHostKey()
                        .SetRepository("{{workflow.parameters.src_repo}}")
                        .SetSshPrivateKeySecret("git-secret", "ssh-privatekey")
                        .Up()
                    .Up()
                .Up()
            .SetContainer()
                .SetImage(_runConfig.Images?.Git ?? "alpine/git:v2.32.0")
                .SetWorkingDirectory("/mnt/persistence/src")
                .SetCommand("sh", "-c")
                    .AddArgument("echo hash: {{workflow.parameters.hash}} && git checkout {{workflow.parameters.hash}} && git status && ls -l")
                .AddVolumeMount("persistence", "/mnt/persistence")
                .Up();
        // @formatter:on
    }

    public void ConfigureDAG(DAGSpecBuilder dagSpecBuilder)
    {
        // @formatter:off
        dagSpecBuilder
            .AddTask()
                .SetName(Name)
                .SetTemplate(Name);
        // @formatter:on
    }
}