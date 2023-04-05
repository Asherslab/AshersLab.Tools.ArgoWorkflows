using AshersLab.Tools.ArgoWorkflows.Interfaces;

namespace AshersLab.Tools.ArgoWorkflows.Models.BuildSteps;

public class DockerBuildStep : IBuildStep
{
    public string        DockerFile   { get; }
    public List<string>  DockerImages { get; }
    public List<string>? Platforms    { get; set; }

    public DockerBuildStep(
        string        dockerFile,
        List<string>  dockerImages,
        List<string>? platforms
    )
    {
        DockerFile = dockerFile;
        DockerImages = dockerImages;
        Platforms = platforms;
    }
}