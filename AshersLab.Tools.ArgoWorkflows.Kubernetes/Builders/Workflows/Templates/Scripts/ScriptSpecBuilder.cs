using AshersLab.Tools.ArgoWorkflows.Kubernetes.Builders.Workflows.Templates.Containers;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Interfaces;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Models.ResourceSpecs.Workflows.Templates.Containers;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Models.ResourceSpecs.Workflows.Templates.Scripts;

namespace AshersLab.Tools.ArgoWorkflows.Kubernetes.Builders.Workflows.Templates.Scripts;

public class ScriptSpecBuilder : ContainerBuilder<ScriptTemplateBuilder>, IBuilder<ScriptSpec>
{
    private string? _source;

    public ScriptSpecBuilder(ScriptTemplateBuilder parent) : base(parent)
    {
    }

    public ScriptSpecBuilder SetSource(string source)
    {
        _source = source;
        return this;
    }

    public override ScriptSpec Build()
    {
        TemplateContainer templateContainer = base.Build();

        if (_source == null)
            throw new InvalidOperationException("Source must be set");

        return new ScriptSpec(
            _source,
            templateContainer
        );
    }
}