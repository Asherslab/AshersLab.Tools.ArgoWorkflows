using System.Text.Json.Serialization;

namespace AshersLab.Tools.ArgoWorkflows.Kubernetes.Models.ResourceSpecs.Workflows;

public class WorkflowArguments
{
    [JsonPropertyName("parameters")]
    public IEnumerable<WorkflowArgumentsParameter> Parameters { get; set; }

    public WorkflowArguments(IEnumerable<WorkflowArgumentsParameter> parameters)
    {
        Parameters = parameters;
    }
}