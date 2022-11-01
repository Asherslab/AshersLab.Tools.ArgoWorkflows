using System.Text.Json.Serialization;

namespace AshersLab.Tools.ArgoWorkflows.Kubernetes.Models.ResourceSpecs.Workflows.Templates.Volumes;

public class TemplateVolume
{
    [JsonPropertyName("name")]
    public string Name { get; }
    
    [JsonPropertyName("secret")]
    public VolumeSecretSpec? SecretSpec { get; }

    public TemplateVolume(string name, VolumeSecretSpec secretSpec)
    {
        Name = name;
        SecretSpec = secretSpec;
    }
}