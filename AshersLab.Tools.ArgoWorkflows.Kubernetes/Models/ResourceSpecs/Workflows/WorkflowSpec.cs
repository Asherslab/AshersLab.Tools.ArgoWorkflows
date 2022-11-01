using System.Text.Json.Serialization;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Interfaces.Kubernetes;

namespace AshersLab.Tools.ArgoWorkflows.Kubernetes.Models.ResourceSpecs.Workflows;

public class WorkflowSpec : IResourceSpec
{
    [JsonPropertyName("entrypoint")]
    public string Entrypoint { get; }

    [JsonPropertyName("arguments")]
    public WorkflowArguments? Arguments { get; }

    [JsonPropertyName("volumeClaimTemplates")]
    public IEnumerable<WorkflowVolumeClaimTemplate>? VolumeClaimTemplates { get; }

    [JsonPropertyName("templates")]
    public IEnumerable<object> Templates { get; }

    public WorkflowSpec(
        string                                    entrypoint,
        WorkflowArguments?                        arguments,
        IEnumerable<WorkflowVolumeClaimTemplate>? volumeClaimTemplates,
        IEnumerable<IWorkflowTemplate>           templates)
    {
        Entrypoint = entrypoint;
        Arguments = arguments;
        VolumeClaimTemplates = volumeClaimTemplates;
        Templates = templates;
    }
}