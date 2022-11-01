using AshersLab.Tools.ArgoWorkflows.Interfaces;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Builders.Workflows;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Builders.Workflows.Templates.DAG;

namespace AshersLab.Tools.ArgoWorkflows.Services.BuildSteps;

public class DAGEntrypointService : IEntrypointService
{
    public const string Name = "execute";
    
    private readonly IEnumerable<IBuildStepService> _buildStepServices;

    public DAGEntrypointService(IEnumerable<IBuildStepService> buildStepServices)
    {
        _buildStepServices = buildStepServices;
    }

    public void ConfigureWorkflow(WorkflowBuilder workflowBuilder)
    {
        DAGSpecBuilder specBuilder = workflowBuilder
            .SetEntrypoint(Name)
            .AddDAGTemplate()
            .SetName(Name)
            .SetSpec();

        foreach (IBuildStepService buildStepService in _buildStepServices)
        {
            buildStepService.ConfigureDAG(specBuilder);
        }
    }
}