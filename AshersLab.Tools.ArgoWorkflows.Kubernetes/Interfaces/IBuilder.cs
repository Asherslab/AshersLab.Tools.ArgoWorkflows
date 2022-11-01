namespace AshersLab.Tools.ArgoWorkflows.Kubernetes.Interfaces;

public interface IBuilder<T>
{
    public T Build();
}