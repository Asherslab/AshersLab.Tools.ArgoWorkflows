using System.Text.Json.Serialization;

namespace AshersLab.Tools.ArgoWorkflows.Kubernetes.Models.ResourceSpecs.Workflows.Templates.Containers;

public class ContainerResourcesData
{
    [JsonPropertyName("memory")]
    public string? Memory { get; set; }

    [JsonPropertyName("cpu")]
    public string? Cpu { get; set; }

    public ContainerResourcesData(
        string? memory,
        string? cpu
    )
    {
        Memory = memory;
        Cpu = cpu;
    }
}