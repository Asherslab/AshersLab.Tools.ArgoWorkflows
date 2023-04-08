using System.Text.Json.Serialization;

namespace AshersLab.Tools.ArgoWorkflows.Kubernetes.Models.ResourceSpecs.Workflows;

public class WorkflowVolumePVC
{
    [JsonPropertyName("claimName")]
    public string ClaimName { get; set; }

    public WorkflowVolumePVC(
        string claimName
    )
    {
        ClaimName = claimName;
    }
}