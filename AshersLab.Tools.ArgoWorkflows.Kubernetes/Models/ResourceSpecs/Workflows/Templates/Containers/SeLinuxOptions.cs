using System.Text.Json.Serialization;

namespace AshersLab.Tools.ArgoWorkflows.Kubernetes.Models.ResourceSpecs.Workflows.Templates.Containers;

public class SeLinuxOptions
{
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    public SeLinuxOptions(string? type)
    {
        Type = type;
    }
}