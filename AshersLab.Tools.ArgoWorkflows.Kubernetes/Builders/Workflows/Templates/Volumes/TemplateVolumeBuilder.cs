using AshersLab.Tools.ArgoWorkflows.Kubernetes.Interfaces;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Models;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Models.ResourceSpecs.Workflows.Templates.Volumes;

namespace AshersLab.Tools.ArgoWorkflows.Kubernetes.Builders.Workflows.Templates.Volumes;

public class TemplateVolumeBuilder<TParent> : NestedBuilder<TParent>, IBuilder<TemplateVolume>
{
    private string?                     _name;
    private IBuilder<VolumeSecretSpec>? _secretBuilder;

    public TemplateVolumeBuilder(TParent parent) : base(parent)
    {
    }

    public TemplateVolumeBuilder<TParent> SetName(string name)
    {
        _name = name;
        return this;
    }

    public VolumeSecretSpecBuilder<TParent> SetSecret()
    {
        VolumeSecretSpecBuilder<TParent> builder = new(this);
        _secretBuilder = builder;
        return builder;
    }

    public TemplateVolumeBuilder<TParent> SetSecret(string name)
    {
        SetSecret().SetName(name);
        return this;
    }

    public TemplateVolume Build()
    {
        if (_name == null)
            throw new InvalidOperationException("Name must be set");

        if (_secretBuilder == null)
            throw new InvalidOperationException("Secret must be set");

        return new TemplateVolume(
            _name,
            _secretBuilder.Build()
        );
    }
}