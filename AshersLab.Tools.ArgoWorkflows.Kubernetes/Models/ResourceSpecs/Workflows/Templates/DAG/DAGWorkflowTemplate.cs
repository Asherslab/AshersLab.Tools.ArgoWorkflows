using System.Text.Json.Serialization;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Interfaces.Kubernetes;

namespace AshersLab.Tools.ArgoWorkflows.Kubernetes.Models.ResourceSpecs.Workflows.Templates.DAG;

public class DAGWorkflowTemplate : IWorkflowTemplate
{
    [JsonPropertyName("name")]
    public string Name { get; }
    
    [JsonPropertyName("dag")]
    public DAGSpec Spec { get; }

    public DAGWorkflowTemplate(string name, DAGSpec spec)
    {
        Name = name;
        Spec = spec;
    }
}