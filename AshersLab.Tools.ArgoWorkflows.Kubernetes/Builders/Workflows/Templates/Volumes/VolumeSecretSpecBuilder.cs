using AshersLab.Tools.ArgoWorkflows.Kubernetes.Interfaces;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Models;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Models.ResourceSpecs.Workflows.Templates.Volumes;

namespace AshersLab.Tools.ArgoWorkflows.Kubernetes.Builders.Workflows.Templates.Volumes;

public class VolumeSecretSpecBuilder<TParent> : NestedBuilder<TemplateVolumeBuilder<TParent>>, IBuilder<VolumeSecretSpec>
{
    private string? _name;
    
    public VolumeSecretSpecBuilder(TemplateVolumeBuilder<TParent> parent) : base(parent)
    {
    }

    public VolumeSecretSpecBuilder<TParent> SetName(string name)
    {
        _name = name;
        return this;
    }

    public VolumeSecretSpec Build()
    {
        if (_name == null)
            throw new InvalidOperationException("Name must be set");

        return new VolumeSecretSpec(_name);
    }
}