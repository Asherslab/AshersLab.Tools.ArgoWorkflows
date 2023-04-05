using AshersLab.Tools.ArgoWorkflows.Interfaces;
using AshersLab.Tools.ArgoWorkflows.Interfaces.Services;
using AshersLab.Tools.ArgoWorkflows.Models;
using AshersLab.Tools.ArgoWorkflows.Models.BuildSteps;
using Microsoft.Build.Construction;
using Microsoft.Build.Evaluation;

namespace AshersLab.Tools.ArgoWorkflows.Services;

public class ProjectInSolutionHandler : IProjectInSolutionHandler
{
    public bool Predicate(ProjectInSolution project)
    {
        return project.ProjectType == SolutionProjectType.KnownToBeMSBuildFormat;
    }

    public IEnumerable<IProject> Handle(string solutionDirectory, IEnumerable<ProjectInSolution> projects)
    {
        foreach (ProjectInSolution projectInSolution in projects)
        {
            Project csProject = ProjectCollection.GlobalProjectCollection.LoadProject(projectInSolution.AbsolutePath);

            List<string> relativeDependencies = new();
            foreach (ProjectItem projectReference in csProject.GetItems("ProjectReference"))
            {
                string referencedFileName = Path.Combine(csProject.DirectoryPath, projectReference.EvaluatedInclude);
                referencedFileName = Path.GetFullPath(referencedFileName);
                relativeDependencies.Add(referencedFileName.Replace($"{solutionDirectory}/", ""));
            }

            List<IBuildStep> buildSteps = new()
            {
                new DotnetBuildStep()
            };

            string? dockerFile = csProject.GetPropertyValue("DockerFile");

            List<string> dockerImages = csProject.AllEvaluatedProperties
                .Where(x => x.Name == "DockerImageName")
                .Select(x => x.EvaluatedValue)
                .ToList();

            List<string>? platforms = csProject.AllEvaluatedProperties
                .Where(x => x.Name == "DockerPlatform")
                .Select(x => x.EvaluatedValue)
                .ToList();

            if (!platforms.Any())
                platforms = null;

            if (dockerFile != null && File.Exists(Path.Combine(csProject.DirectoryPath, dockerFile)) &&
                dockerImages.Any())
            {
                buildSteps.Add(new DockerBuildStep(dockerFile, dockerImages, platforms));

                string? deploymentRepo             = csProject.GetPropertyValue("DeploymentRepo");
                string? deploymentRepoDirectory    = csProject.GetPropertyValue("DeploymentRepoDirectory");
                string? deploymentImage            = csProject.GetPropertyValue("DeploymentImage");
                string? deploymentImageReplacement = csProject.GetPropertyValue("DeploymentImageReplacement");

                if (string.IsNullOrWhiteSpace(deploymentRepoDirectory))
                    deploymentRepoDirectory = null;

                if (!string.IsNullOrWhiteSpace(deploymentRepo) &&
                    !string.IsNullOrWhiteSpace(deploymentImage) &&
                    !string.IsNullOrWhiteSpace(deploymentImageReplacement))
                {
                    buildSteps.Add(
                        new DeployDockerStep(
                            deploymentRepo,
                            deploymentRepoDirectory,
                            deploymentImage,
                            deploymentImageReplacement
                        )
                    );
                }
            }

            string? deployNuget = csProject.GetPropertyValue("DeployNuget");
            if (deployNuget != null && bool.TryParse(deployNuget, out bool deploy) && deploy)
            {
                buildSteps.Add(new NugetStep());
            }

            yield return new SimpleProject(
                projectInSolution.ProjectName,
                projectInSolution.RelativePath,
                csProject,
                csProject.GetPropertyValue("TargetFramework"),
                relativeDependencies,
                buildSteps
            );
        }
    }
}