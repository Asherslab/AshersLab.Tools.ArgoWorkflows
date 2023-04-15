using System.Text.Json.Serialization;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Interfaces.Kubernetes;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Models.ResourceSpecs.Workflows.Templates.Volumes;

namespace AshersLab.Tools.ArgoWorkflows.Kubernetes.Models.ResourceSpecs.Workflows.Templates.Containers;

public class ContainerWorkflowTemplate : IWorkflowTemplate
{
    [JsonPropertyName("name")]
    public string Name { get; }

    [JsonPropertyName("container")]
    public TemplateContainer Container { get; }

    [JsonPropertyName("inputs")]
    public TemplateInputs? Inputs { get; }

    [JsonPropertyName("volumes")]
    public IEnumerable<TemplateVolume>? Volumes { get; }

    [JsonPropertyName("serviceAccountName")]
    public string? ServiceAccountName { get; set; }

    public ContainerWorkflowTemplate(
        string                       name,
        TemplateContainer            container,
        TemplateInputs?              inputs             = null,
        IEnumerable<TemplateVolume>? volumes            = null,
        string?                      serviceAccountName = null
    )
    {
        Name = name;
        Container = container;
        Inputs = inputs;
        Volumes = volumes;
        ServiceAccountName = serviceAccountName;
    }
}