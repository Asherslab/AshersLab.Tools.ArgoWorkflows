using System.Text.Json.Serialization;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Interfaces.Kubernetes;

namespace AshersLab.Tools.ArgoWorkflows.Kubernetes.Models.ResourceSpecs.VolumeClaims;

public class VolumeClaimSpec : IResourceSpec
{
    [JsonPropertyName("accessModes")]
    public IEnumerable<AccessModes> AccessModes { get; set; }

    [JsonPropertyName("resources")]
    public VolumeClaimResources Resources { get; set; }

    public VolumeClaimSpec(IEnumerable<AccessModes> accessModes, VolumeClaimResources resources)
    {
        AccessModes = accessModes;
        Resources = resources;
    }
}