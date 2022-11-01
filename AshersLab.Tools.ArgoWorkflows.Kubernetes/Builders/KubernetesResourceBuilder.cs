using AshersLab.Tools.ArgoWorkflows.Kubernetes.Builders.Workflows;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Interfaces;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Interfaces.Kubernetes;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Models;

namespace AshersLab.Tools.ArgoWorkflows.Kubernetes.Builders;

public class KubernetesResourceBuilder : IBuilder<KubernetesResource>
{
    private string?                       _apiVersion;
    private string?                       _kind;
    private IBuilder<KubernetesMetadata>? _metadataBuilder;
    private IBuilder<IResourceSpec>?  _resourceSpecBuilder;

    public MetadataBuilder<KubernetesResourceBuilder> SetMetadata()
    {
        MetadataBuilder<KubernetesResourceBuilder> metadataBuilder = new(this);
        _metadataBuilder = metadataBuilder;
        return metadataBuilder;
    }

    public WorkflowBuilder SetAsWorkflow()
    {
        if (_kind != null)
            throw new InvalidOperationException("Resource kind has already been set");

        _apiVersion = "argoproj.io/v1alpha1";
        _kind = "Workflow";
        
        WorkflowBuilder workflowBuilder = new(this);
        _resourceSpecBuilder = workflowBuilder;
        return workflowBuilder;
    }

    public KubernetesResource Build()
    {
        if (_apiVersion == null || _kind == null || _resourceSpecBuilder == null)
            throw new InvalidOperationException("Resource Spec must be set");

        if (_metadataBuilder == null)
            throw new InvalidOperationException("Metadata must be set");

        return new KubernetesResource(
            _apiVersion,
            _kind,
            _metadataBuilder.Build(),
            _resourceSpecBuilder.Build()
        );
    }
}