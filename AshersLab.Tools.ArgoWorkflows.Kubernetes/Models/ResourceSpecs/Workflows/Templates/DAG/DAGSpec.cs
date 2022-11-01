using System.Text.Json.Serialization;

namespace AshersLab.Tools.ArgoWorkflows.Kubernetes.Models.ResourceSpecs.Workflows.Templates.DAG;

public class DAGSpec
{
    [JsonPropertyName("tasks")]
    public IEnumerable<DAGTask> Tasks { get; }

    public DAGSpec(IEnumerable<DAGTask> tasks)
    {
        Tasks = tasks;
    }
}