using AshersLab.Tools.ArgoWorkflows.Kubernetes.Interfaces;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Models;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Models.ResourceSpecs.Workflows.Templates.DAG;

namespace AshersLab.Tools.ArgoWorkflows.Kubernetes.Builders.Workflows.Templates.DAG;

public class DAGSpecBuilder : NestedBuilder<DAGTemplateBuilder>, IBuilder<DAGSpec>
{
    private ICollection<IBuilder<DAGTask>>? _dagTaskBuilders;

    public DAGSpecBuilder(DAGTemplateBuilder parent) : base(parent)
    {
    }

    public DAGTaskBuilder AddTask()
    {
        _dagTaskBuilders ??= new List<IBuilder<DAGTask>>();
        DAGTaskBuilder builder = new(this);
        _dagTaskBuilders.Add(builder);
        return builder;
    }

    public DAGTaskBuilder AddTask(string name, string template)
    {
        DAGTaskBuilder builder = AddTask()
            .SetName(name)
            .SetTemplate(template);
        return builder;
    }

    public DAGSpec Build()
    {
        if (_dagTaskBuilders == null)
            throw new InvalidOperationException("Must have one or more Tasks");

        return new DAGSpec(_dagTaskBuilders.Select(x => x.Build()));
    }
}