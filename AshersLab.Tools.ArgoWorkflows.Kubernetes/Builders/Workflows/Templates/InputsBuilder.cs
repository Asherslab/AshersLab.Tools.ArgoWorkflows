using AshersLab.Tools.ArgoWorkflows.Kubernetes.Builders.Workflows.Templates.Artifacts;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Interfaces;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Interfaces.Kubernetes;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Models;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Models.ResourceSpecs.Workflows.Templates;

namespace AshersLab.Tools.ArgoWorkflows.Kubernetes.Builders.Workflows.Templates;

public class InputsBuilder<TParent> : NestedBuilder<TParent>, IBuilder<TemplateInputs>
{
    private ICollection<IBuilder<IArtifact>>? _artifactBuilders;

    public InputsBuilder(TParent parent) : base(parent)
    {
    }

    public GitArtifactBuilder<TParent> AddGitArtifact()
    {
        _artifactBuilders ??= new List<IBuilder<IArtifact>>();
        GitArtifactBuilder<TParent> builder = new(this);
        _artifactBuilders.Add(builder);
        return builder;
    }

    public TemplateInputs Build()
    {
        if (_artifactBuilders == null)
            throw new InvalidOperationException("Must have one or more Artifacts");

        return new TemplateInputs(
            _artifactBuilders.Select(x => x.Build())
        );
    }
}