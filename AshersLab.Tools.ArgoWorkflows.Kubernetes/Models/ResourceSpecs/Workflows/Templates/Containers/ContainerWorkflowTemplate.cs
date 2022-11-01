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

    public ContainerWorkflowTemplate(
        string                     name,
        TemplateContainer          container,
        TemplateInputs?            inputs  = null,
        IEnumerable<TemplateVolume>? volumes = null
    )
    {
        Name = name;
        Container = container;
        Inputs = inputs;
        Volumes = volumes;
    }
}