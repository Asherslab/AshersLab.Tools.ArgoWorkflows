using AshersLab.Tools.ArgoWorkflows.Kubernetes.Builders.Workflows;

namespace AshersLab.Tools.ArgoWorkflows.Interfaces;

public interface IEntrypointService
{
    public void ConfigureWorkflow(WorkflowBuilder workflowBuilder);
}