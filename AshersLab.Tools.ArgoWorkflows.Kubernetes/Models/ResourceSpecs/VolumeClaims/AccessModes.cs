using System.Text.Json.Serialization;

namespace AshersLab.Tools.ArgoWorkflows.Kubernetes.Models.ResourceSpecs.VolumeClaims;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum AccessModes
{
    ReadWriteOnce,
    ReadWriteMany
}