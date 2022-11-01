using AshersLab.Tools.ArgoWorkflows.Kubernetes.Builders.Workflows;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Interfaces;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Models;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Models.ResourceSpecs.VolumeClaims;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Models.ResourceSpecs.Workflows;

namespace AshersLab.Tools.ArgoWorkflows.Kubernetes.Builders.VolumeClaims;

public class WorkflowVolumeClaimBuilder : NestedBuilder<WorkflowBuilder>, IBuilder<WorkflowVolumeClaimTemplate>
{
    private IBuilder<KubernetesMetadata>? _metadataBuilder;
    private IBuilder<VolumeClaimSpec>?    _volumeBuilder;

    public WorkflowVolumeClaimBuilder(WorkflowBuilder parent) : base(parent)
    {
    }

    public MetadataBuilder<WorkflowVolumeClaimBuilder> SetMetadata()
    {
        MetadataBuilder<WorkflowVolumeClaimBuilder> metadataBuilder = new(this);
        _metadataBuilder = metadataBuilder;
        return metadataBuilder;
    }

    public VolumeClaimBuilder SetVolumeClaim()
    {
        VolumeClaimBuilder volumeClaimBuilder = new(this);
        _volumeBuilder = volumeClaimBuilder;
        return volumeClaimBuilder;
    }

    public WorkflowVolumeClaimTemplate Build()
    {
        if (_metadataBuilder == null)
            throw new InvalidOperationException("Metadata must be set");

        if (_volumeBuilder == null)
            throw new InvalidOperationException("Volume Claim must be set");

        return new WorkflowVolumeClaimTemplate(
            _metadataBuilder.Build(),
            _volumeBuilder.Build()
        );
    }
}