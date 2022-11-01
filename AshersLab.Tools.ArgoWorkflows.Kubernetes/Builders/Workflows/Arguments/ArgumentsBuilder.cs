using AshersLab.Tools.ArgoWorkflows.Kubernetes.Interfaces;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Models;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Models.ResourceSpecs.Workflows;

namespace AshersLab.Tools.ArgoWorkflows.Kubernetes.Builders.Workflows.Arguments;

public class ArgumentsBuilder : NestedBuilder<WorkflowBuilder>, IBuilder<WorkflowArguments>
{
    private ICollection<IBuilder<WorkflowArgumentsParameter>>? _parameterBuilders;

    public ArgumentsBuilder(WorkflowBuilder parent) : base(parent)
    {
    }

    public ArgumentsParameterBuilder AddParameter()
    {
        _parameterBuilders ??= new List<IBuilder<WorkflowArgumentsParameter>>();
        ArgumentsParameterBuilder builder = new(this);
        _parameterBuilders.Add(builder);
        return builder;
    }

    public WorkflowArguments Build()
    {
        if (_parameterBuilders == null)
            throw new InvalidOperationException("Must have one or more Parameters");

        return new WorkflowArguments(
            _parameterBuilders.Select(x => x.Build())
        );
    }
}