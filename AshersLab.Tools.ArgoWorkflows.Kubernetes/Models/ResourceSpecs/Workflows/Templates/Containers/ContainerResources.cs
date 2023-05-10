using System.Text.Json.Serialization;

namespace AshersLab.Tools.ArgoWorkflows.Kubernetes.Models.ResourceSpecs.Workflows.Templates.Containers;

public class ContainerResources
{
    [JsonPropertyName("limits")]
    public ContainerResourcesData? Limits { get; set; }

    [JsonPropertyName("requests")]
    public ContainerResourcesData? Requests { get; set; }

    public ContainerResources(
        ContainerResourcesData? limits = null,
        ContainerResourcesData? requests = null
    )
    {
        Limits = limits;
        Requests = requests;
    }
}