using AshersLab.Tools.ArgoWorkflows.Kubernetes.Interfaces;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Models;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Models.ResourceSpecs.Workflows.Templates;

namespace AshersLab.Tools.ArgoWorkflows.Kubernetes.Builders.Workflows.Templates;

public class VolumeMountBuilder<TParent> : NestedBuilder<TParent>, IBuilder<VolumeMount>
{
    private string? _name;
    private string? _mountPath;
    
    public VolumeMountBuilder(TParent parent) : base(parent)
    {
    }

    public VolumeMountBuilder<TParent> SetName(string name)
    {
        _name = name;
        return this;
    }

    public VolumeMountBuilder<TParent> SetMountPath(string path)
    {
        _mountPath = path;
        return this;
    }

    public VolumeMount Build()
    {
        if (_name == null)
            throw new InvalidOperationException("Name must be set");

        if (_mountPath == null)
            throw new InvalidOperationException("Mount Path must be set");

        return new VolumeMount(_name, _mountPath);
    }
}