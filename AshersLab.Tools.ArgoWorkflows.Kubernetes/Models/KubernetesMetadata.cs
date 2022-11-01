using System.Text.Json.Serialization;

namespace AshersLab.Tools.ArgoWorkflows.Kubernetes.Models;

public class KubernetesMetadata
{
    [JsonPropertyName("generateName")]
    public string? GenerateName { get; set; }

    [JsonPropertyName("namespace")]
    public string? Namespace { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    public KubernetesMetadata(
        string? generateName = null,
        string? ns = null,
        string? name = null
    )
    {
        GenerateName = generateName;
        Namespace = ns;
        Name = name;
    }
}