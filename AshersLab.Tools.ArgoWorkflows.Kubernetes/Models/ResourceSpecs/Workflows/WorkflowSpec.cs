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

    [JsonPropertyName("volumes")]
    public IEnumerable<WorkflowVolume>? Volumes { get; }

    [JsonPropertyName("templates")]
    public IEnumerable<object> Templates { get; }

    [JsonPropertyName("parallelism")]
    public int? Parallelism { get; }

    [JsonPropertyName("tolerations")]
    public IEnumerable<WorkflowToleration>? Tolerations { get; set; }

    [JsonPropertyName("nodeSelector")]
    public IDictionary<string, string>? NodeSelector { get; set; }

    public WorkflowSpec(
        string                                    entrypoint,
        WorkflowArguments?                        arguments,
        IEnumerable<WorkflowVolumeClaimTemplate>? volumeClaimTemplates,
        IEnumerable<WorkflowVolume>?              volumes,
        IEnumerable<IWorkflowTemplate>            templates,
        int?                                      parallelism  = null,
        IEnumerable<WorkflowToleration>?          tolerations  = null,
        IDictionary<string, string>?              nodeSelector = null
    )
    {
        Entrypoint = entrypoint;
        Arguments = arguments;
        VolumeClaimTemplates = volumeClaimTemplates;
        Volumes = volumes;
        Templates = templates;
        Parallelism = parallelism;
        Tolerations = tolerations;
        NodeSelector = nodeSelector;
    }
}