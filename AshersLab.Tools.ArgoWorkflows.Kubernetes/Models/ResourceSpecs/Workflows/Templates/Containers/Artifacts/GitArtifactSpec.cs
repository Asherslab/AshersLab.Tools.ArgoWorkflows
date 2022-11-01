using System.Text.Json.Serialization;

namespace AshersLab.Tools.ArgoWorkflows.Kubernetes.Models.ResourceSpecs.Workflows.Templates.Containers.Artifacts;

public class GitArtifactSpec
{
    [JsonPropertyName("repo")]
    public string Repository { get; }

    [JsonPropertyName("insecureIgnoreHostKey")]
    public bool IgnoreHostKey { get; }

    [JsonPropertyName("sshPrivateKeySecret")]
    public GitPrivateKeySecret? SshPrivateKeySecret { get; }

    public GitArtifactSpec(
        string               repository,
        bool                 ignoreHostKey       = false,
        GitPrivateKeySecret? sshPrivateKeySecret = null
    )
    {
        IgnoreHostKey = ignoreHostKey;
        Repository = repository;
        SshPrivateKeySecret = sshPrivateKeySecret;
    }
}