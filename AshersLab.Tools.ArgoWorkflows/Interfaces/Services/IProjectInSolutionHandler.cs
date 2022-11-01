using Microsoft.Build.Construction;

namespace AshersLab.Tools.ArgoWorkflows.Interfaces.Services;

public interface IProjectInSolutionHandler
{
    public bool                  Predicate(ProjectInSolution           project);
    public IEnumerable<IProject> Handle(string solutionDirectory, IEnumerable<ProjectInSolution> projects);
}