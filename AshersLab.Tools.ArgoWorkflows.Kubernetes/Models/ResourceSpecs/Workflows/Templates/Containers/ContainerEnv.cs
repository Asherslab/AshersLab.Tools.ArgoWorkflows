using System.Text.Json.Serialization;

namespace AshersLab.Tools.ArgoWorkflows.Kubernetes.Models.ResourceSpecs.Workflows.Templates.Containers;

public class ContainerEnv
{
    [JsonPropertyName("name")]
    public string Name { get; }
    
    [JsonPropertyName("value")]
    public string? Value { get; }
    
    [JsonPropertyName("valueFrom")]
    public ContainerEnvExternalValue? ExternalValue {get;}

    public ContainerEnv(string name, string value)
    {
        Name = name;
        Value = value;
    }

    public ContainerEnv(string name, ContainerEnvExternalValue externalValue)
    {
        Name = name;
        ExternalValue = externalValue;
    }
}