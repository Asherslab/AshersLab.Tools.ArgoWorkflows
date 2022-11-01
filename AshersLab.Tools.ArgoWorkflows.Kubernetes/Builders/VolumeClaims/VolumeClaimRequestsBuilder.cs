using AshersLab.Tools.ArgoWorkflows.Kubernetes.Interfaces;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Models;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Models.ResourceSpecs.VolumeClaims;

namespace AshersLab.Tools.ArgoWorkflows.Kubernetes.Builders.VolumeClaims;

public class VolumeClaimRequestsBuilder : NestedBuilder<VolumeClaimResourcesBuilder>, IBuilder<VolumeClaimRequests>
{
    private int?          _amount;
    private StorageSizes? _size;

    public VolumeClaimRequestsBuilder(VolumeClaimResourcesBuilder parent) : base(parent)
    {
    }

    public VolumeClaimRequestsBuilder SetAmount(int amount)
    {
        _amount = amount;
        return this;
    }

    public VolumeClaimRequestsBuilder SetSize(StorageSizes size)
    {
        _size = size;
        return this;
    }

    public VolumeClaimRequests Build()
    {
        if (_amount == null)
            throw new InvalidOperationException("Amount must be set");

        if (_size == null)
            throw new InvalidOperationException("Size must be set");

        return new VolumeClaimRequests($"{_amount}{_size.ToString()}");
    }
}