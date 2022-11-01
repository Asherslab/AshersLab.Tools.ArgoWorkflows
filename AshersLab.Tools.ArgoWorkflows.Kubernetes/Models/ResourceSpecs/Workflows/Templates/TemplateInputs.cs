using System.Text.Json.Serialization;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Interfaces.Kubernetes;

namespace AshersLab.Tools.ArgoWorkflows.Kubernetes.Models.ResourceSpecs.Workflows.Templates;

public class TemplateInputs
{
    [JsonPropertyName("artifacts")]
    public IEnumerable<object> Artifacts { get; }

    public TemplateInputs(IEnumerable<IArtifact> artifacts)
    {
        Artifacts = artifacts;
    }
}