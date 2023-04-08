using AshersLab.Tools.ArgoWorkflows.Kubernetes.Builders.VolumeClaims;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Builders.Workflows.Arguments;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Builders.Workflows.Templates.DAG;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Builders.Workflows.Templates.Scripts;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Interfaces;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Interfaces.Kubernetes;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Models;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Models.ResourceSpecs.Workflows;

namespace AshersLab.Tools.ArgoWorkflows.Kubernetes.Builders.Workflows;

public class WorkflowBuilder : NestedBuilder<KubernetesResourceBuilder>, IBuilder<IResourceSpec>
{
    private string?                                             _entrypoint;
    private ArgumentsBuilder?                                   _workflowArgumentsBuilder;
    private ICollection<IBuilder<WorkflowVolumeClaimTemplate>>? _workflowVolumeClaimBuilders;
    private ICollection<IBuilder<WorkflowVolume>>?              _workflowVolumeBuilders;
    private ICollection<IBuilder<IWorkflowTemplate>>?           _workflowTemplateBuilders;
    private int?                                                _parallelism;

    public WorkflowBuilder(KubernetesResourceBuilder parent) : base(parent)
    {
    }

    public WorkflowBuilder SetEntrypoint(string entrypoint)
    {
        _entrypoint = entrypoint;
        return this;
    }

    public ArgumentsBuilder SetArguments()
    {
        _workflowArgumentsBuilder = new ArgumentsBuilder(this);
        return _workflowArgumentsBuilder;
    }

    public WorkflowBuilder SetParallelism(int? parallelism)
    {
        _parallelism = parallelism;
        return this;
    }

    public WorkflowBuilder AddArgumentsParameter(string name, string value)
    {
        _workflowArgumentsBuilder ??= new ArgumentsBuilder(this);
        _workflowArgumentsBuilder
            .AddParameter()
            .SetName(name)
            .SetValue(value);
        return this;
    }

    public WorkflowVolumeClaimBuilder AddWorkflowVolumeClaim()
    {
        _workflowVolumeClaimBuilders ??= new List<IBuilder<WorkflowVolumeClaimTemplate>>();
        WorkflowVolumeClaimBuilder builder = new(this);
        _workflowVolumeClaimBuilders.Add(builder);
        return builder;
    }

    public WorkflowVolumeBuilder AddWorkflowVolume()
    {
        _workflowVolumeBuilders ??= new List<IBuilder<WorkflowVolume>>();
        WorkflowVolumeBuilder builder = new(this);
        _workflowVolumeBuilders.Add(builder);
        return builder;
    }

    public WorkflowBuilder AddWorkflowVolume(string name, string pvcName)
    {
        AddWorkflowVolume()
            .SetName(name)
            .SetPersistentVolumeClaimName(pvcName);
        return this;
    }

    public ScriptTemplateBuilder AddScriptTemplate()
    {
        _workflowTemplateBuilders ??= new List<IBuilder<IWorkflowTemplate>>();
        ScriptTemplateBuilder builder = new(this);
        _workflowTemplateBuilders.Add(builder);
        return builder;
    }

    public ContainerTemplateBuilder<WorkflowBuilder> AddContainerTemplate()
    {
        _workflowTemplateBuilders ??= new List<IBuilder<IWorkflowTemplate>>();
        ContainerTemplateBuilder<WorkflowBuilder> builder = new(this);
        _workflowTemplateBuilders.Add(builder);
        return builder;
    }

    public DAGTemplateBuilder AddDAGTemplate()
    {
        _workflowTemplateBuilders ??= new List<IBuilder<IWorkflowTemplate>>();
        DAGTemplateBuilder builder = new(this);
        _workflowTemplateBuilders.Add(builder);
        return builder;
    }

    public IResourceSpec Build()
    {
        if (_entrypoint == null)
            throw new InvalidOperationException("Entrypoint must be set");

        if (_workflowTemplateBuilders == null)
            throw new InvalidOperationException("Must have one or more Workflow Templates");

        ICollection<IWorkflowTemplate> workflowTemplates = _workflowTemplateBuilders.Select(x => x.Build()).ToList();

        if (workflowTemplates.All(x => x.Name != _entrypoint))
            throw new InvalidOperationException("No Workflow Templates matching the Entrypoint");

        if (_parallelism is < 1)
            throw new InvalidOperationException("Parallelism must be higher than 0");

        return new WorkflowSpec(
            _entrypoint,
            _workflowArgumentsBuilder?.Build(),
            _workflowVolumeClaimBuilders?.Select(x => x.Build()),
            _workflowVolumeBuilders?.Select(x => x.Build()),
            workflowTemplates,
            _parallelism
        );
    }
}