using AshersLab.Tools.ArgoWorkflows.Kubernetes.Interfaces;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Models;

namespace AshersLab.Tools.ArgoWorkflows.Kubernetes.Builders;

public class MetadataBuilder<TParent> : NestedBuilder<TParent>, IBuilder<KubernetesMetadata>
{
    private string? _generateName;
    private string? _namespace;
    private string? _name;
    
    public MetadataBuilder(TParent parent) : base(parent)
    {
    }
    
    public MetadataBuilder<TParent> SetGenerateName(string name)
    {
        _generateName = name;
        return this;
    }
    
    public MetadataBuilder<TParent> SetNamespace(string kubernetesNamespace)
    {
        _namespace = kubernetesNamespace;
        return this;
    }

    public MetadataBuilder<TParent> SetName(string name)
    {
        _name = name;
        return this;
    }

    public KubernetesMetadata Build()
    {
        return new KubernetesMetadata(
            _generateName,
            _namespace,
            _name
        );
    }
}