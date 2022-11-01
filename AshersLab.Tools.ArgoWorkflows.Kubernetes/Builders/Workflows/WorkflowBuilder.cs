using AshersLab.Tools.ArgoWorkflows.Kubernetes.Builders.VolumeClaims;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Builders.Workflows.Arguments;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Builders.Workflows.Templates.Containers;
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
    private ICollection<IBuilder<WorkflowVolumeClaimTemplate>>? _workflowVolumesBuilder;
    private ICollection<IBuilder<IWorkflowTemplate>>?           _workflowTemplatesBuilder;
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

    public WorkflowVolumeClaimBuilder AddWorkflowVolume()
    {
        _workflowVolumesBuilder ??= new List<IBuilder<WorkflowVolumeClaimTemplate>>();
        WorkflowVolumeClaimBuilder builder = new(this);
        _workflowVolumesBuilder.Add(builder);
        return builder;
    }

    public ScriptTemplateBuilder AddScriptTemplate()
    {
        _workflowTemplatesBuilder ??= new List<IBuilder<IWorkflowTemplate>>();
        ScriptTemplateBuilder builder = new(this);
        _workflowTemplatesBuilder.Add(builder);
        return builder;
    }

    public ContainerTemplateBuilder<WorkflowBuilder> AddContainerTemplate()
    {
        _workflowTemplatesBuilder ??= new List<IBuilder<IWorkflowTemplate>>();
        ContainerTemplateBuilder<WorkflowBuilder> builder = new(this);
        _workflowTemplatesBuilder.Add(builder);
        return builder;
    }
    
    public DAGTemplateBuilder AddDAGTemplate()
    {
        _workflowTemplatesBuilder ??= new List<IBuilder<IWorkflowTemplate>>();
        DAGTemplateBuilder builder = new(this);
        _workflowTemplatesBuilder.Add(builder);
        return builder;
    }

    public IResourceSpec Build()
    {
        if (_entrypoint == null)
            throw new InvalidOperationException("Entrypoint must be set");

        if (_workflowTemplatesBuilder == null)
            throw new InvalidOperationException("Must have one or more Workflow Templates");

        ICollection<IWorkflowTemplate> workflowTemplates = _workflowTemplatesBuilder.Select(x => x.Build()).ToList();

        if (workflowTemplates.All(x => x.Name != _entrypoint))
            throw new InvalidOperationException("No Workflow Templates matching the Entrypoint");

        if (_parallelism is < 1)
            throw new InvalidOperationException("Parallelism must be higher than 0");

        return new WorkflowSpec(
            _entrypoint,
            _workflowArgumentsBuilder?.Build(),
            _workflowVolumesBuilder?.Select(x => x.Build()),
            workflowTemplates,
            _parallelism
        );
    }
}