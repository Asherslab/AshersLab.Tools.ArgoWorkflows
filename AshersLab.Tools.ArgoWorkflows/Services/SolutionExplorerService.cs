using AshersLab.Tools.ArgoWorkflows.Interfaces;
using AshersLab.Tools.ArgoWorkflows.Interfaces.Services;
using Microsoft.Build.Construction;

namespace AshersLab.Tools.ArgoWorkflows.Services;

public class SolutionExplorerService
{
    private IEnumerable<IProjectInSolutionHandler> _handlers;

    public SolutionExplorerService(
        IEnumerable<IProjectInSolutionHandler> handlers
    )
    {
        _handlers = handlers;
    }

    public IEnumerable<IProject> Explore(string location)
    {
        SolutionFile solution = SolutionFile.Parse(location);

        List<ProjectInSolution> projectsToHandle = solution.ProjectsInOrder.ToList();

        foreach (IProjectInSolutionHandler handler in _handlers)
        {
            List<ProjectInSolution> handledProjects = projectsToHandle
                .Where(handler.Predicate)
                .ToList();
            projectsToHandle = projectsToHandle.Except(handledProjects).ToList();
            foreach (IProject project in handler.Handle(Path.GetDirectoryName(location)!, handledProjects))
            {
                yield return project;
            }
        }
    }
}