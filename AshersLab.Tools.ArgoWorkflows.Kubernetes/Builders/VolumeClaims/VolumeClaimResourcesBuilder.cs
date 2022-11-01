using AshersLab.Tools.ArgoWorkflows.Kubernetes.Interfaces;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Models;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Models.ResourceSpecs.VolumeClaims;

namespace AshersLab.Tools.ArgoWorkflows.Kubernetes.Builders.VolumeClaims;

public class VolumeClaimResourcesBuilder : NestedBuilder<VolumeClaimBuilder>, IBuilder<VolumeClaimResources>
{
    private IBuilder<VolumeClaimRequests>? _requestsBuilder;

    public VolumeClaimResourcesBuilder(VolumeClaimBuilder parent) : base(parent)
    {
    }

    public VolumeClaimRequestsBuilder SetRequests()
    {
        VolumeClaimRequestsBuilder builder = new(this);
        _requestsBuilder = builder;
        return builder;
    }

    public VolumeClaimResources Build()
    {
        if (_requestsBuilder == null)
            throw new InvalidOperationException("Requests must be set");

        return new VolumeClaimResources(_requestsBuilder.Build());
    }
}