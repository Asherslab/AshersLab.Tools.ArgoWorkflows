using System.Text.Json.Serialization;

namespace AshersLab.Tools.ArgoWorkflows.Kubernetes.Models.ResourceSpecs.Workflows.Templates.Containers;

public class ContainerEnvExternalValue
{
    [JsonPropertyName("secretKeyRef")]
    public ExternalValueSecretReference? Secret {get;}

    public ContainerEnvExternalValue(ExternalValueSecretReference? secret)
    {
        Secret = secret;
    }
}