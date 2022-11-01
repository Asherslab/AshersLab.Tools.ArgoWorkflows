using System.Text.Json.Serialization;

namespace AshersLab.Tools.ArgoWorkflows.Kubernetes.Models.ResourceSpecs.Workflows.Templates.Containers;

public class ExternalValueSecretReference
{
    [JsonPropertyName("name")]
    public string Name { get; }
    
    [JsonPropertyName("key")]
    public string Key { get; }

    public ExternalValueSecretReference(string name, string key)
    {
        Name = name;
        Key = key;
    }
}