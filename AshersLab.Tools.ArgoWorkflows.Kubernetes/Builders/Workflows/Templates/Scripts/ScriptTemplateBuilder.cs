using AshersLab.Tools.ArgoWorkflows.Kubernetes.Builders.Workflows.Templates.Volumes;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Interfaces;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Interfaces.Kubernetes;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Models;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Models.ResourceSpecs.Workflows.Templates;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Models.ResourceSpecs.Workflows.Templates.Scripts;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Models.ResourceSpecs.Workflows.Templates.Volumes;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Extensions;

namespace AshersLab.Tools.ArgoWorkflows.Kubernetes.Builders.Workflows.Templates.Scripts;

public class ScriptTemplateBuilder : NestedBuilder<WorkflowBuilder>, IBuilder<IWorkflowTemplate>
{
    private string?                                _name;
    private IBuilder<ScriptSpec>?                  _specBuilder;
    private IBuilder<TemplateInputs>?              _inputsBuilder;
    private ICollection<IBuilder<TemplateVolume>>? _volumeBuilder;

    public ScriptTemplateBuilder(WorkflowBuilder parent) : base(parent)
    {
    }
    
    public ScriptTemplateBuilder SetName(string name)
    {
        _name = name;
        return this;
    }

    public ScriptSpecBuilder SetScript()
    {
        ScriptSpecBuilder builder = new(this);
        _specBuilder = builder;
        return builder;
    }

    public InputsBuilder<ScriptTemplateBuilder> SetInputs()
    {
        InputsBuilder<ScriptTemplateBuilder> builder = new(this);
        _inputsBuilder = builder;
        return builder;
    }

    public TemplateVolumeBuilder<ScriptTemplateBuilder> AddVolume()
    {
        _volumeBuilder ??= new List<IBuilder<TemplateVolume>>();
        TemplateVolumeBuilder<ScriptTemplateBuilder> builder = new(this);
        _volumeBuilder.Add(builder);
        return builder;
    }

    public IWorkflowTemplate Build()
    {
        if (_name == null)
            throw new InvalidOperationException("Name must be set");

        if (_specBuilder == null)
            throw new InvalidOperationException("Script must be set");

        return new ScriptWorkflowTemplate(
            _name.ArgoNormalize(),
            _specBuilder.Build(),
            _inputsBuilder?.Build(),
            _volumeBuilder?.Select(x => x.Build())
        );
    }
}