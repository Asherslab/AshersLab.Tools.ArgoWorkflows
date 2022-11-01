using System.Text.Json.Serialization;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global

namespace AshersLab.Tools.ArgoWorkflows.Kubernetes.Models.ResourceSpecs.VolumeClaims;

public class VolumeClaimRequests
{
    [JsonPropertyName("storage")]
    public string Storage { get; }

    public VolumeClaimRequests(string storage)
    {
        Storage = storage;
    }
}