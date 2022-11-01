using System.Text.Json.Serialization;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Interfaces.Kubernetes;

namespace AshersLab.Tools.ArgoWorkflows.Kubernetes.Models;

public class KubernetesResource
{
    [JsonPropertyName("apiVersion")]
    public string ApiVersion { get; }

    [JsonPropertyName("kind")]
    public string Kind { get; }

    [JsonPropertyName("metadata")]
    public KubernetesMetadata Metadata { get; }

    [JsonPropertyName("spec")]
    public object Spec { get; }

    public KubernetesResource(
        string             apiVersion,
        string             kind,
        KubernetesMetadata metadata,
        IResourceSpec      spec
    )
    {
        ApiVersion = apiVersion;
        Kind = kind;
        Metadata = metadata;
        Spec = spec;
    }
}