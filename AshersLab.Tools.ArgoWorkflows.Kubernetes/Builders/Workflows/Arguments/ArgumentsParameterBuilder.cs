using AshersLab.Tools.ArgoWorkflows.Kubernetes.Interfaces;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Models;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Models.ResourceSpecs.Workflows;

namespace AshersLab.Tools.ArgoWorkflows.Kubernetes.Builders.Workflows.Arguments;

public class ArgumentsParameterBuilder : NestedBuilder<ArgumentsBuilder>, IBuilder<WorkflowArgumentsParameter>
{
    private string? _name;
    private string? _value;

    public ArgumentsParameterBuilder(ArgumentsBuilder parent) : base(parent)
    {
    }

    public ArgumentsParameterBuilder SetName(string name)
    {
        _name = name;
        return this;
    }

    public ArgumentsParameterBuilder SetValue(string value)
    {
        _value = value;
        return this;
    }

    public WorkflowArgumentsParameter Build()
    {
        if (_name == null)
            throw new InvalidOperationException("Name must be set");

        if (_value == null)
            throw new InvalidOperationException("Value must be set");

        return new WorkflowArgumentsParameter(
            _name,
            _value
        );
    }
}