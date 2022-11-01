using AshersLab.Tools.ArgoWorkflows.Kubernetes.Interfaces;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Interfaces.Kubernetes;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Models;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Models.ResourceSpecs.Workflows.Templates.DAG;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Extensions;

namespace AshersLab.Tools.ArgoWorkflows.Kubernetes.Builders.Workflows.Templates.DAG;

public class DAGTemplateBuilder : NestedBuilder<WorkflowBuilder>, IBuilder<IWorkflowTemplate>
{
    private string?                         _name;
    private IBuilder<DAGSpec>?              _specBuilder;

    public DAGTemplateBuilder(WorkflowBuilder parent) : base(parent)
    {
    }

    public DAGTemplateBuilder SetName(string name)
    {
        _name = name;
        return this;
    }

    public DAGSpecBuilder SetSpec()
    {
        DAGSpecBuilder dagSpecBuilder = new(this);
        _specBuilder = dagSpecBuilder;
        return dagSpecBuilder;
    }

    public IWorkflowTemplate Build()
    {
        if (_name == null)
            throw new InvalidOperationException("Name must be set");

        if (_specBuilder == null)
            throw new InvalidOperationException("Spec must be set");

        return new DAGWorkflowTemplate(
            _name.ArgoNormalize(),
            _specBuilder.Build()
        );
    }
}