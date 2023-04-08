using System.Text.Json.Serialization;

namespace AshersLab.Tools.ArgoWorkflows.Kubernetes.Models.ResourceSpecs.Workflows;

public class WorkflowVolume
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("persistentVolumeClaim")]
    public WorkflowVolumePVC PersistentVolumeClaim { get; set; }

    public WorkflowVolume(
        string            name,
        WorkflowVolumePVC persistentVolumeClaim
    )
    {
        Name = name;
        PersistentVolumeClaim = persistentVolumeClaim;
    }
}