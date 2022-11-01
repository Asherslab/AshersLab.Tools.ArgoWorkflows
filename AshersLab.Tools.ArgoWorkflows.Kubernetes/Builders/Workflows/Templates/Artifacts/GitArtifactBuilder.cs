using AshersLab.Tools.ArgoWorkflows.Kubernetes.Interfaces;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Interfaces.Kubernetes;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Models;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Models.ResourceSpecs.Workflows.Templates.Containers.Artifacts;

namespace AshersLab.Tools.ArgoWorkflows.Kubernetes.Builders.Workflows.Templates.Artifacts;

public class GitArtifactBuilder<TParent> : NestedBuilder<InputsBuilder<TParent>>, IBuilder<IArtifact>
{
    private string?                           _name;
    private string?                           _path;
    private IBuilder<GitArtifactSpec>? _specBuilder;

    public GitArtifactBuilder(InputsBuilder<TParent> parent) : base(parent)
    {
    }

    public GitArtifactBuilder<TParent> SetName(string name)
    {
        _name = name;
        return this;
    }

    public GitArtifactBuilder<TParent> SetPath(string path)
    {
        _path = path;
        return this;
    }

    public GitArtifactSpecBuilder<TParent> SetSpec()
    {
        GitArtifactSpecBuilder<TParent> builder = new(this);
        _specBuilder = builder;
        return builder;
    }

    public IArtifact Build()
    {
        if (_name == null)
            throw new InvalidOperationException("Name must be set");

        if (_path == null)
            throw new InvalidOperationException("Path must be set");

        if (_specBuilder == null)
            throw new InvalidOperationException("Spec must be set");


        return new GitArtifact(
            _name,
            _path,
            _specBuilder.Build()
        );
    }
}