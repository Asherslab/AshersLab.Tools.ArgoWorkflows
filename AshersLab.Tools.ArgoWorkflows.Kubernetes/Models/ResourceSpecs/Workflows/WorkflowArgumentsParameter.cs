using System.Text.Json.Serialization;

namespace AshersLab.Tools.ArgoWorkflows.Kubernetes.Models.ResourceSpecs.Workflows;

public class WorkflowArgumentsParameter
{
    [JsonPropertyName("name")]
    public string Name { get; }

    [JsonPropertyName("value")]
    public string Value { get; }

    public WorkflowArgumentsParameter(string name, string value)
    {
        Name = name;
        Value = value;
    }
}