using AshersLab.Tools.ArgoWorkflows.Kubernetes.Models.ResourceSpecs.VolumeClaims;

namespace AshersLab.Tools.ArgoWorkflows.Configuration;

public class RunConfig
{
    public string SolutionFile { get; set; }
    public string TargetHash   { get; set; }
    public string SourceRepo   { get; set; }

    public string  PersistenceVolumePath { get; set; } = "/mnt/persistence";
    public string? ToolOutput            { get; set; } //= "/mnt/out/workflow";
    public string  RepoDirectory         { get; set; } = "/mnt/persistence/src";

    public string[]? NugetSources { get; set; }

    public string NugetPushUrl { get; set; }
    public string? JqToolUrl { get; set; }
    public string? SemverToolUrl { get; set; }
    public string GitDeployCommitEmail { get; set; }

    public ImagesConfig? Images { get; set; } = new();

    public int         PersistentVolumeSize { get; set; } = 100;
    public int?        MaxParallelism       { get; set; }
    public AccessModes VolumeAccessMode     { get; set; } = AccessModes.ReadWriteMany;
}