using AshersLab.Tools.ArgoWorkflows.Kubernetes.Builders.Workflows.Templates.Volumes;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Interfaces;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Interfaces.Kubernetes;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Models;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Models.ResourceSpecs.Workflows.Templates;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Models.ResourceSpecs.Workflows.Templates.Containers;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Models.ResourceSpecs.Workflows.Templates.Volumes;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Extensions;

namespace AshersLab.Tools.ArgoWorkflows.Kubernetes.Builders.Workflows.Templates.Containers;

public class ContainerTemplateBuilder<TParent> : NestedBuilder<WorkflowBuilder>, IBuilder<IWorkflowTemplate>
{
    private string?                                _name;
    private IBuilder<TemplateContainer>?           _containerBuilder;
    private IBuilder<TemplateInputs>?              _inputsBuilder;
    private ICollection<IBuilder<TemplateVolume>>? _volumeBuilder;

    public ContainerTemplateBuilder(WorkflowBuilder parent) : base(parent)
    {
    }

    public ContainerTemplateBuilder<TParent> SetName(string name)
    {
        _name = name;
        return this;
    }

    public ContainerBuilder<ContainerTemplateBuilder<TParent>> SetContainer()
    {
        ContainerBuilder<ContainerTemplateBuilder<TParent>> builder = new(this);
        _containerBuilder = builder;
        return builder;
    }

    public InputsBuilder<ContainerTemplateBuilder<TParent>> SetInputs()
    {
        InputsBuilder<ContainerTemplateBuilder<TParent>> builder = new(this);
        _inputsBuilder = builder;
        return builder;
    }

    public TemplateVolumeBuilder<ContainerTemplateBuilder<TParent>> AddVolume()
    {
        _volumeBuilder ??= new List<IBuilder<TemplateVolume>>();
        TemplateVolumeBuilder<ContainerTemplateBuilder<TParent>> builder = new(this);
        _volumeBuilder.Add(builder);
        return builder;
    }

    public IWorkflowTemplate Build()
    {
        if (_name == null)
            throw new InvalidOperationException("Name must be set");

        if (_containerBuilder == null)
            throw new InvalidOperationException("Container must be set");

        return new ContainerWorkflowTemplate(
            _name.ArgoNormalize(),
            _containerBuilder.Build(),
            _inputsBuilder?.Build(),
            _volumeBuilder?.Select(x => x.Build())
        );
    }
}