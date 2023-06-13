using System.Text.Json.Serialization;

namespace AshersLab.Tools.ArgoWorkflows.Kubernetes.Models.ResourceSpecs.Workflows.Templates.Containers;

public class ContainerSecurityContext
{
    [JsonPropertyName("privileged")]
    public bool Privileged { get; }

    [JsonPropertyName("seLinuxOptions")]
    public SeLinuxOptions? SeLinuxOptions { get; set; }

    public ContainerSecurityContext(bool privileged = false, SeLinuxOptions? seLinuxOptions = null)
    {
        Privileged = privileged;
        SeLinuxOptions = seLinuxOptions;
    }
}