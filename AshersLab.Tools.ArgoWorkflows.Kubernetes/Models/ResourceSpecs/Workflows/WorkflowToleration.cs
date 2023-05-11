using System.Text.Json.Serialization;

namespace AshersLab.Tools.ArgoWorkflows.Kubernetes.Models.ResourceSpecs.Workflows;

public class WorkflowToleration
{
    [JsonPropertyName("key")]
    public string Key { get; set; }

    [JsonPropertyName("operator")]
    public string Operator { get; set; }

    [JsonPropertyName("value")]
    public string Value { get; set; }

    [JsonPropertyName("effect")]
    public string Effect { get; set; }

    public WorkflowToleration(
        string key,
        string @operator,
        string value,
        string effect
    )
    {
        Key = key;
        Operator = @operator;
        Value = value;
        Effect = effect;
    }

    public WorkflowToleration()
    {
        Key = null!;
        Operator = null!;
        Value = null!;
        Effect = null!;
    }
}