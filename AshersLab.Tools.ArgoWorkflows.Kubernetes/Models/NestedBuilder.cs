namespace AshersLab.Tools.ArgoWorkflows.Kubernetes.Models;

public abstract class NestedBuilder<TParent>
{
    private readonly TParent _parent;

    protected NestedBuilder(TParent parent)
    {
        _parent = parent;
    }

    public TParent Up()
    {
        return _parent;
    }
}