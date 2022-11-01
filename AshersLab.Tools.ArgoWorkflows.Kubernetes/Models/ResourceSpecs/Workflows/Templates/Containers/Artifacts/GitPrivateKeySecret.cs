using System.Text.Json.Serialization;

namespace AshersLab.Tools.ArgoWorkflows.Kubernetes.Models.ResourceSpecs.Workflows.Templates.Containers.Artifacts;

public class GitPrivateKeySecret
{
    [JsonPropertyName("name")]
    public string Name { get; }
    
    [JsonPropertyName("key")]
    public string Key { get; }

    public GitPrivateKeySecret(string name, string key)
    {
        Name = name;
        Key = key;
    }
}