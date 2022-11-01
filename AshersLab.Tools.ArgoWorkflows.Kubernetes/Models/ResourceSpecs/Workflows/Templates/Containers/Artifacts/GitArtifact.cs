using System.Text.Json.Serialization;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Interfaces.Kubernetes;

namespace AshersLab.Tools.ArgoWorkflows.Kubernetes.Models.ResourceSpecs.Workflows.Templates.Containers.Artifacts;

public class GitArtifact : IArtifact
{
    [JsonPropertyName("name")]
    public string Name { get; }

    [JsonPropertyName("path")]
    public string Path { get; }

    [JsonPropertyName("git")]
    public GitArtifactSpec Git { get; }

    public GitArtifact(string name, string path, GitArtifactSpec git)
    {
        Name = name;
        Path = path;
        Git = git;
    }
}