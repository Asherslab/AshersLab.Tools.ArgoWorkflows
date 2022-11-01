using AshersLab.Tools.ArgoWorkflows.Interfaces;
using Microsoft.Build.Evaluation;

namespace AshersLab.Tools.ArgoWorkflows.Models;

public class SimpleProject : IProject
{
    public string           Name             { get; }
    public string           RelativeLocation { get; }
    public Project          Project          { get; }
    public string           TargetFramework  { get; }
    public List<string>     Dependencies     { get; }
    public List<IBuildStep> BuildSteps       { get; }

    public SimpleProject(
        string           name,
        string           relativeLocation,
        Project          project,
        string           targetFramework,
        List<string>     dependencies,
        List<IBuildStep> buildSteps
    )
    {
        Name = name;
        RelativeLocation = relativeLocation;
        Project = project;
        TargetFramework = targetFramework;
        Dependencies = dependencies;
        BuildSteps = buildSteps;
    }
}