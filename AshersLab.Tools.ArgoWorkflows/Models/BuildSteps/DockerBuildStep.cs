using AshersLab.Tools.ArgoWorkflows.Interfaces;

namespace AshersLab.Tools.ArgoWorkflows.Models.BuildSteps;

public class DockerBuildStep : IBuildStep
{
    public string       DockerFile   { get; }
    public List<string> DockerImages { get; }

    public DockerBuildStep(
        string       dockerFile,
        List<string> dockerImages
    )
    {
        DockerFile = dockerFile;
        DockerImages = dockerImages;
    }
}