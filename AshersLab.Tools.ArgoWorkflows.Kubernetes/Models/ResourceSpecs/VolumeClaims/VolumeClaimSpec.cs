using System.Text.Json.Serialization;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Interfaces.Kubernetes;

namespace AshersLab.Tools.ArgoWorkflows.Kubernetes.Models.ResourceSpecs.VolumeClaims;

public class VolumeClaimSpec : IResourceSpec
{
    [JsonPropertyName("storageClassName")]
    public string? StorageClassName { get; set; }

    [JsonPropertyName("accessModes")]
    public IEnumerable<AccessModes> AccessModes { get; set; }

    [JsonPropertyName("resources")]
    public VolumeClaimResources Resources { get; set; }

    public VolumeClaimSpec(string? storageClassName, IEnumerable<AccessModes> accessModes, VolumeClaimResources resources)
    {
        StorageClassName = storageClassName;
        AccessModes = accessModes;
        Resources = resources;
    }
}