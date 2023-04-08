using AshersLab.Tools.ArgoWorkflows.Kubernetes.Interfaces;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Models;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Models.ResourceSpecs.Workflows;

namespace AshersLab.Tools.ArgoWorkflows.Kubernetes.Builders.Workflows;

public class WorkflowVolumeBuilder : NestedBuilder<WorkflowBuilder>, IBuilder<WorkflowVolume>
{
    public string? _name;
    public string? _persistentVolumeClaimName;

    public WorkflowVolumeBuilder(WorkflowBuilder parent) : base(parent)
    {
    }

    public WorkflowVolumeBuilder SetName(string name)
    {
        _name = name;
        return this;
    }

    public WorkflowVolumeBuilder SetPersistentVolumeClaimName(string name)
    {
        _persistentVolumeClaimName = name;
        return this;
    }

    public WorkflowVolume Build()
    {
        if (_name == null)
            throw new InvalidOperationException("Name must be set");

        if (_persistentVolumeClaimName == null)
            throw new InvalidOperationException("Persistent Volume Claim Name must be set");

        return new WorkflowVolume(
            _name,
            new WorkflowVolumePVC(
                _persistentVolumeClaimName
            )
        );
    }
}