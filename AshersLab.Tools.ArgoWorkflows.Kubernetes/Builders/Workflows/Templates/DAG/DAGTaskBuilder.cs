using AshersLab.Tools.ArgoWorkflows.Kubernetes.Interfaces;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Models;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Models.ResourceSpecs.Workflows.Templates.DAG;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Extensions;

namespace AshersLab.Tools.ArgoWorkflows.Kubernetes.Builders.Workflows.Templates.DAG;

public class DAGTaskBuilder : NestedBuilder<DAGSpecBuilder>, IBuilder<DAGTask>
{
    private string?              _name;
    private string?              _template;
    private ICollection<string>? _dependencies;

    public DAGTaskBuilder(DAGSpecBuilder parent) : base(parent)
    {
    }

    public DAGTaskBuilder SetName(string name)
    {
        _name = name;
        return this;
    }

    public DAGTaskBuilder SetTemplate(string template)
    {
        _template = template;
        return this;
    }

    public DAGTaskBuilder AddDependency(string dependency)
    {
        _dependencies ??= new List<string>();
        _dependencies.Add(dependency);
        return this;
    }

    public DAGTask Build()
    {
        if (_name == null)
            throw new InvalidOperationException("Name must be set");

        if (_template == null)
            throw new InvalidOperationException("Template must be set");

        return new DAGTask(
            _name.ArgoNormalize(),
            _template.ArgoNormalize(),
            _dependencies?.Select(x => x.ArgoNormalize())
        );
    }
}