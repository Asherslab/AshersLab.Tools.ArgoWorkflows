using System.Text.Json.Serialization;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Models.ResourceSpecs.VolumeClaims;

namespace AshersLab.Tools.ArgoWorkflows.Kubernetes.Models.ResourceSpecs.Workflows;

public class WorkflowVolumeClaimTemplate
{
    [JsonPropertyName("metadata")]
    public KubernetesMetadata Metadata { get; set; }
        
    [JsonPropertyName("spec")]
    public VolumeClaimSpec Spec { get; set; }

    public WorkflowVolumeClaimTemplate(KubernetesMetadata metadata, VolumeClaimSpec spec)
    {
        Metadata = metadata;
        Spec = spec;
    }
}