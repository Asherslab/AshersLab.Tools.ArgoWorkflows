using System.Text.Json.Serialization;

namespace AshersLab.Tools.ArgoWorkflows.Kubernetes.Models.ResourceSpecs.Workflows.Templates.DAG;

public class DAGTask
{
    [JsonPropertyName("name")]
    public string Name { get; }

    [JsonPropertyName("template")]
    public string Template { get; }

    [JsonPropertyName("dependencies")]
    public IEnumerable<string>? Dependencies { get; }

    public DAGTask(string name, string template, IEnumerable<string>? dependencies = null)
    {
        Name = name;
        Template = template;
        Dependencies = dependencies;
    }
}