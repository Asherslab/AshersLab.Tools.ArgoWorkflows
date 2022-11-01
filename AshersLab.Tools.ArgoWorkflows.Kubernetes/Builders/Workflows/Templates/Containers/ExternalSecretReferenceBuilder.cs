using AshersLab.Tools.ArgoWorkflows.Kubernetes.Interfaces;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Models;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Models.ResourceSpecs.Workflows.Templates.Containers;

namespace AshersLab.Tools.ArgoWorkflows.Kubernetes.Builders.Workflows.Templates.Containers;

public class ExternalSecretReferenceBuilder<TParent> : NestedBuilder<TParent>, IBuilder<ExternalValueSecretReference>
{
    private string? _name;
    private string? _key;

    public ExternalSecretReferenceBuilder(TParent parent) : base(parent)
    {
    }

    public ExternalSecretReferenceBuilder<TParent> SetName(string name)
    {
        _name = name;
        return this;
    }

    public ExternalSecretReferenceBuilder<TParent> SetKey(string key)
    {
        _key = key;
        return this;
    }

    public ExternalValueSecretReference Build()
    {
        if (_name == null)
            throw new InvalidOperationException("Name must be set");

        if (_key == null)
            throw new InvalidOperationException("Key must be set");

        return new ExternalValueSecretReference(_name, _key);
    }
}