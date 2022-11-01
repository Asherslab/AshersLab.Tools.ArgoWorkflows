using System.Text.Json.Serialization;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Models.ResourceSpecs.Workflows.Templates.Containers;

namespace AshersLab.Tools.ArgoWorkflows.Kubernetes.Models.ResourceSpecs.Workflows.Templates.Scripts;

public class ScriptSpec : TemplateContainer
{
    [JsonPropertyName("source")]
    public string Source { get; }

    public ScriptSpec(
        string            source,
        TemplateContainer container
    ) : base(container.Image, container.Command, container.Arguments, container.WorkingDirectory,
        container.VolumeMounts, container.EnvironmentVariables, container.SecurityContext)
    {
        Source = source;
    }
}