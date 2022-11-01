using AshersLab.Tools.ArgoWorkflows.Kubernetes.Interfaces;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Models;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Models.ResourceSpecs.Workflows.Templates.Containers;

namespace AshersLab.Tools.ArgoWorkflows.Kubernetes.Builders.Workflows.Templates.Containers;

public class ContainerEnvExternalValueBuilder<TParent> : NestedBuilder<TParent>, IBuilder<ContainerEnvExternalValue>
{
    private IBuilder<ExternalValueSecretReference>? _secretBuilder;

    public ContainerEnvExternalValueBuilder(TParent parent) : base(parent)
    {
    }

    public ExternalSecretReferenceBuilder<ContainerEnvExternalValueBuilder<TParent>> SetSecret()
    {
        ExternalSecretReferenceBuilder<ContainerEnvExternalValueBuilder<TParent>> builder = new(this);
        _secretBuilder = builder;
        return builder;
    }

    public ContainerEnvExternalValue Build()
    {
        if (_secretBuilder == null)
            throw new InvalidOperationException("Secret must be set");

        return new ContainerEnvExternalValue(_secretBuilder.Build());
    }
}