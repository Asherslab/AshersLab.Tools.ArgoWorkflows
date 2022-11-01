using AshersLab.Tools.ArgoWorkflows.Kubernetes.Interfaces;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Models;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Models.ResourceSpecs.Workflows.Templates.Containers.Artifacts;

namespace AshersLab.Tools.ArgoWorkflows.Kubernetes.Builders.Workflows.Templates.Artifacts;

public class GitPrivateKeySecretBuilder<TParent> : NestedBuilder<GitArtifactSpecBuilder<TParent>>, IBuilder<GitPrivateKeySecret>
{
    private string? _name;
    private string? _key;
    
    public GitPrivateKeySecretBuilder(GitArtifactSpecBuilder<TParent> parent) : base(parent)
    {
    }

    public GitPrivateKeySecretBuilder<TParent> SetName(string name)
    {
        _name = name;
        return this;
    }

    public GitPrivateKeySecretBuilder<TParent> SetKey(string key)
    {
        _key = key;
        return this;
    }

    public GitPrivateKeySecret Build()
    {
        if (_name == null)
            throw new InvalidOperationException("Name must be set");

        if (_key == null)
            throw new InvalidOperationException("Key must be set");

        return new GitPrivateKeySecret(_name, _key);
    }
}