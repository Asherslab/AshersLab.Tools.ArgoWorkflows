using System.Text.Json.Serialization;

namespace AshersLab.Tools.ArgoWorkflows.Kubernetes.Models.ResourceSpecs.Workflows.Templates.Volumes;

public class VolumeSecretSpec
{
    [JsonPropertyName("secretName")]
    public string Name { get; }

    public VolumeSecretSpec(string name)
    {
        Name = name;
    }
}