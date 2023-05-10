using AshersLab.Tools.ArgoWorkflows.Kubernetes.Interfaces;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Models;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Models.ResourceSpecs.Workflows.Templates.Containers;

namespace AshersLab.Tools.ArgoWorkflows.Kubernetes.Builders.Workflows.Templates.Containers;

public class ContainerResourcesBuilder<TParent> : NestedBuilder<TParent>, IBuilder<ContainerResources?>
{
    private ContainerResourcesData? _requests;
    private ContainerResourcesData? _limits;

    public ContainerResourcesBuilder(TParent parent) : base(parent)
    {
    }

    public ContainerResourcesBuilder<TParent> SetRequests(string? memory, string? cpu)
    {
        if (memory == null && cpu == null)
            return this;

        _requests = new ContainerResourcesData(memory, cpu);
        return this;
    }

    public ContainerResourcesBuilder<TParent> SetLimits(string? memory, string? cpu)
    {
        if (memory == null && cpu == null)
            return this;

        _limits = new ContainerResourcesData(memory, cpu);
        return this;
    }

    public ContainerResources? Build()
    {
        if (_limits == null && _requests == null)
            return null;

        return new ContainerResources(_limits, _requests);
    }
}