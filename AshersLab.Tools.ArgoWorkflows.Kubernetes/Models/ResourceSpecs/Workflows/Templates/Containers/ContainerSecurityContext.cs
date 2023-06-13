using System.Text.Json.Serialization;

namespace AshersLab.Tools.ArgoWorkflows.Kubernetes.Models.ResourceSpecs.Workflows.Templates.Containers;

public class ContainerSecurityContext
{
    [JsonPropertyName("privileged")]
    public bool Privileged { get; }

    public ContainerSecurityContext(bool privileged = false)
    {
        Privileged = privileged;
    }
}