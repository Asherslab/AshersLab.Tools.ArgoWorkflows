using AshersLab.Tools.ArgoWorkflows.Kubernetes.Interfaces;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Models;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Models.ResourceSpecs.VolumeClaims;

namespace AshersLab.Tools.ArgoWorkflows.Kubernetes.Builders.VolumeClaims;

public class VolumeClaimBuilder : NestedBuilder<WorkflowVolumeClaimBuilder>, IBuilder<VolumeClaimSpec>
{
    private string?                         _storageClassName;
    private ICollection<AccessModes>?       _accessModes;
    private IBuilder<VolumeClaimResources>? _resourcesBuilder;

    public VolumeClaimBuilder(WorkflowVolumeClaimBuilder parent) : base(parent)
    {
    }

    public VolumeClaimBuilder SetStorageClassName(string? storageClassName)
    {
        _storageClassName = storageClassName;
        return this;
    }

    public VolumeClaimBuilder AddAccessMode(AccessModes mode)
    {
        _accessModes ??= new List<AccessModes>();
        _accessModes.Add(mode);
        return this;
    }

    public VolumeClaimResourcesBuilder SetResources()
    {
        VolumeClaimResourcesBuilder builder = new(this);
        _resourcesBuilder = builder;
        return builder;
    }

    public VolumeClaimBuilder SetResources(int amount, StorageSizes sizes)
    {
        VolumeClaimResourcesBuilder builder = new(this);
        builder.SetRequests()
            .SetAmount(amount)
            .SetSize(sizes);
        _resourcesBuilder = builder;
        return this;
    }

    public VolumeClaimSpec Build()
    {
        if (_accessModes == null)
            throw new InvalidOperationException("Must have one or more Access Mode");

        if (_resourcesBuilder == null)
            throw new InvalidOperationException("Resources must be set");

        return new VolumeClaimSpec(
            _storageClassName,
            _accessModes,
            _resourcesBuilder.Build()
        );
    }
}