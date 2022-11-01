using AshersLab.Tools.ArgoWorkflows.Interfaces;

namespace AshersLab.Tools.ArgoWorkflows.Models.BuildSteps;

public class DeployDockerStep : IBuildStep
{
    public string  DeploymentRepo             { get; }
    public string? DeploymentRepoDirectory    { get; }
    public string  DeploymentImage            { get; }
    public string  DeploymentImageReplacement { get; }

    public DeployDockerStep(
        string  deploymentRepo,
        string? deploymentRepoDirectory,
        string  deploymentImage,
        string  deploymentImageReplacement
    )
    {
        DeploymentRepo = deploymentRepo;
        DeploymentRepoDirectory = deploymentRepoDirectory;
        DeploymentImage = deploymentImage;
        DeploymentImageReplacement = deploymentImageReplacement;
    }
}