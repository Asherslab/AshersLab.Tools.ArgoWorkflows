using System.Text.Json.Serialization;

namespace AshersLab.Tools.ArgoWorkflows.Kubernetes.Models.ResourceSpecs.Workflows.Templates;

public class VolumeMount
{
    [JsonPropertyName("name")]
    public string Name { get; }
    
    [JsonPropertyName("mountPath")]
    public string MountPath { get; }

    public VolumeMount(string name, string mountPath)
    {
        Name = name;
        MountPath = mountPath;
    }
}