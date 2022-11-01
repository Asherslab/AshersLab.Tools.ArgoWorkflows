using AshersLab.Tools.ArgoWorkflows.Kubernetes.Interfaces;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Models;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Models.ResourceSpecs.Workflows.Templates.Containers.Artifacts;

namespace AshersLab.Tools.ArgoWorkflows.Kubernetes.Builders.Workflows.Templates.Artifacts;

public class GitArtifactSpecBuilder<TParent> : NestedBuilder<GitArtifactBuilder<TParent>>, IBuilder<GitArtifactSpec>
{
    private string?                        _repository;
    private bool                           _ignoreHostKey;
    private IBuilder<GitPrivateKeySecret>? _sshPrivateKeyBuilder;

    public GitArtifactSpecBuilder(GitArtifactBuilder<TParent> parent) : base(parent)
    {
    }

    public GitArtifactSpecBuilder<TParent> SetRepository(string repo)
    {
        _repository = repo;
        return this;
    }

    public GitArtifactSpecBuilder<TParent> SetIgnoreHostKey(bool ignore = true)
    {
        _ignoreHostKey = ignore;
        return this;
    }

    public GitPrivateKeySecretBuilder<TParent> SetSshPrivateKeySecret()
    {
        GitPrivateKeySecretBuilder<TParent> builder = new(this);
        _sshPrivateKeyBuilder = builder;
        return builder;
    }
    
    public GitArtifactSpecBuilder<TParent> SetSshPrivateKeySecret(string name, string key)
    {
        _sshPrivateKeyBuilder = SetSshPrivateKeySecret()
            .SetName(name)
            .SetKey(key);
        return this;
    }

    public GitArtifactSpec Build()
    {
        if (_repository == null)
            throw new InvalidCastException("Repository must be set");

        return new GitArtifactSpec(
            _repository,
            _ignoreHostKey,
            _sshPrivateKeyBuilder?.Build()
        );
    }
}