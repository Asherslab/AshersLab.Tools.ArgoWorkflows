using System.Text.Json.Serialization;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global

namespace AshersLab.Tools.ArgoWorkflows.Kubernetes.Models.ResourceSpecs.VolumeClaims;

public class VolumeClaimResources
{
    [JsonPropertyName("requests")]
    public VolumeClaimRequests Requests { get; }

    public VolumeClaimResources(VolumeClaimRequests requests)
    {
        Requests = requests;
    }
}