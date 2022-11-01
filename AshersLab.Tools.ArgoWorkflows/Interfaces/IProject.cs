using Microsoft.Build.Evaluation;

namespace AshersLab.Tools.ArgoWorkflows.Interfaces;

public interface IProject
{
    public string           Name              { get; }
    public string           RelativeLocation  { get; }
    public string?          RelativeDirectory => Path.GetDirectoryName(RelativeLocation);
    public Project          Project           { get; }
    public string           TargetFramework   { get; }
    public List<string>     Dependencies      { get; }
    public List<IBuildStep> BuildSteps        { get; }

    public IEnumerable<IProject> GetDependencies(ICollection<IProject> projects, bool all = false)
    {
        if (!all)
            return projects.Where(x => Dependencies.Any(y => x.RelativeLocation.Equals(y)));

        List<IProject> returnProjects = new();
        foreach (IProject project in projects.Where(x => Dependencies.Any(y => x.RelativeLocation.Equals(y))))
        {
            returnProjects.Add(project);
            returnProjects.AddRange(project.GetDependencies(projects.Except(returnProjects).ToList(), true));
        }
        return returnProjects;
    }
}