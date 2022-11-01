namespace AshersLab.Tools.ArgoWorkflows.Kubernetes.Interfaces.Kubernetes;

public interface IArtifact
{
    public string Name { get; }
    public string Path { get; }
}