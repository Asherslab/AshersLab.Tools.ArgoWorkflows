using System.Text.Json.Serialization;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Interfaces.Kubernetes;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Models.ResourceSpecs.Workflows.Templates.Volumes;

namespace AshersLab.Tools.ArgoWorkflows.Kubernetes.Models.ResourceSpecs.Workflows.Templates.Scripts;

public class ScriptWorkflowTemplate : IWorkflowTemplate
{
    [JsonPropertyName("name")]
    public string Name { get; }

    [JsonPropertyName("script")]
    public ScriptSpec Spec { get; }

    [JsonPropertyName("inputs")]
    public TemplateInputs? Inputs { get; }

    [JsonPropertyName("volumes")]
    public IEnumerable<TemplateVolume>? Volumes { get; }

    public ScriptWorkflowTemplate(
        string                       name,
        ScriptSpec                   spec,
        TemplateInputs?              inputs  = null,
        IEnumerable<TemplateVolume>? volumes = null
    )
    {
        Name = name;
        Spec = spec;
        Inputs = inputs;
        Volumes = volumes;
    }
}