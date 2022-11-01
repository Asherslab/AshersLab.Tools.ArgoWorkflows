using AshersLab.Tools.ArgoWorkflows.Kubernetes.Builders.Workflows;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Builders.Workflows.Templates.DAG;

namespace AshersLab.Tools.ArgoWorkflows.Interfaces;

public interface IBuildStepService
{
    public void ConfigureWorkflow(WorkflowBuilder workflowBuilder);
    public void ConfigureDAG(DAGSpecBuilder       dagSpecBuilder);
}