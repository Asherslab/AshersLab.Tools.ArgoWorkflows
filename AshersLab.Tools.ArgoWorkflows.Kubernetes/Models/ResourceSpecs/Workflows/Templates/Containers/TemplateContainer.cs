using System.Text.Json.Serialization;

namespace AshersLab.Tools.ArgoWorkflows.Kubernetes.Models.ResourceSpecs.Workflows.Templates.Containers;

public class TemplateContainer
{
    [JsonPropertyName("image")]
    public string Image { get; }

    [JsonPropertyName("command")]
    public IEnumerable<string>? Command { get; }

    [JsonPropertyName("args")]
    public IEnumerable<string>? Arguments { get; }

    [JsonPropertyName("workingDir")]
    public string? WorkingDirectory { get; }

    [JsonPropertyName("volumeMounts")]
    public IEnumerable<VolumeMount>? VolumeMounts { get; }

    [JsonPropertyName("env")]
    public IEnumerable<ContainerEnv>? EnvironmentVariables { get; }

    [JsonPropertyName("securityContext")]
    public ContainerSecurityContext? SecurityContext { get; }
    
    [JsonPropertyName("resources")]
    public ContainerResources? Resources { get; set; }

    public TemplateContainer(
        string                     image,
        IEnumerable<string>?       command                  = null,
        IEnumerable<string>?       arguments                = null,
        string?                    workingDirectory         = null,
        IEnumerable<VolumeMount>?  volumeMounts             = null,
        IEnumerable<ContainerEnv>? environmentVariables     = null,
        ContainerSecurityContext?  containerSecurityContext = null,
        ContainerResources? resources = null
    )
    {
        Image = image;
        Command = command;
        Arguments = arguments;
        WorkingDirectory = workingDirectory;
        VolumeMounts = volumeMounts;
        EnvironmentVariables = environmentVariables;
        SecurityContext = containerSecurityContext;
        Resources = resources;
    }
}