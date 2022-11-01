using AshersLab.Tools.ArgoWorkflows.Kubernetes.Interfaces;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Models;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Models.ResourceSpecs.Workflows.Templates.Containers;

namespace AshersLab.Tools.ArgoWorkflows.Kubernetes.Builders.Workflows.Templates.Containers;

public class ContainerEnvBuilder<TParent> : NestedBuilder<ContainerBuilder<TParent>>, IBuilder<ContainerEnv>
{
    private string?                                 _name;
    private string?                                 _value;
    private IBuilder<ContainerEnvExternalValue>? _externalValueBuilder;

    public ContainerEnvBuilder(ContainerBuilder<TParent> parent) : base(parent)
    {
    }

    public ContainerEnvBuilder<TParent> SetName(string name)
    {
        _name = name;
        return this;
    }

    public ContainerEnvBuilder<TParent> SetValue(string value)
    {
        _value = value;
        return this;
    }

    public ContainerEnvExternalValueBuilder<ContainerEnvBuilder<TParent>> SetExternalValue()
    {
        ContainerEnvExternalValueBuilder<ContainerEnvBuilder<TParent>> builder = new(this);
        _externalValueBuilder = builder;
        return builder;
    }

    public ContainerEnv Build()
    {
        if (_name == null)
            throw new InvalidOperationException("Name must be set");

        if (_value == null && _externalValueBuilder == null)
            throw new InvalidOperationException("Value or External Value must be set");

        if (_value != null)
            return new ContainerEnv(_name, _value);
        return new ContainerEnv(_name, _externalValueBuilder!.Build());
    }
}