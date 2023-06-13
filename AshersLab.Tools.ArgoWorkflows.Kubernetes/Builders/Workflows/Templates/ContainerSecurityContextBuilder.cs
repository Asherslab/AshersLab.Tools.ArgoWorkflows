using AshersLab.Tools.ArgoWorkflows.Kubernetes.Interfaces;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Models;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Models.ResourceSpecs.Workflows.Templates.Containers;

namespace AshersLab.Tools.ArgoWorkflows.Kubernetes.Builders.Workflows.Templates;

public class ContainerSecurityContextBuilder<TParent> : NestedBuilder<TParent>, IBuilder<ContainerSecurityContext>
{
    private bool _privileged;
    
    public ContainerSecurityContextBuilder(TParent parent) : base(parent)
    {
    }

    public ContainerSecurityContextBuilder<TParent> SetPrivileged(bool privileged = true)
    {
        _privileged = privileged;
        return this;
    }

    public ContainerSecurityContext Build()
    {
        return new ContainerSecurityContext(_privileged);
    }
}