using AshersLab.Tools.ArgoWorkflows.Kubernetes.Interfaces;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Models;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Models.ResourceSpecs.Workflows.Templates;
using AshersLab.Tools.ArgoWorkflows.Kubernetes.Models.ResourceSpecs.Workflows.Templates.Containers;

namespace AshersLab.Tools.ArgoWorkflows.Kubernetes.Builders.Workflows.Templates.Containers;

public class ContainerBuilder<TParent> : NestedBuilder<TParent>, IBuilder<TemplateContainer>
{
    private string?                              _image;
    private IEnumerable<string>?                 _command;
    private ICollection<string>?                 _arguments;
    private string?                              _workingDirectory;
    private ICollection<IBuilder<VolumeMount>>?  _volumeMountBuilders;
    private ICollection<IBuilder<ContainerEnv>>? _envBuilders;
    private IBuilder<ContainerSecurityContext>?  _securityContextBuilder;
    private IBuilder<ContainerResources?>?       _containerResourcesBuilder; 

    public ContainerBuilder(TParent parent) : base(parent)
    {
    }

    public ContainerBuilder<TParent> SetImage(string image)
    {
        _image = image;
        return this;
    }

    public ContainerBuilder<TParent> SetWorkingDirectory(string workingDirectory)
    {
        _workingDirectory = workingDirectory;
        return this;
    }

    public ContainerBuilder<TParent> SetCommand(params string[] commandLines)
    {
        _command = commandLines;
        return this;
    }

    public ContainerBuilder<TParent> AddArgument(string argument)
    {
        _arguments ??= new List<string>();
        _arguments.Add(argument);
        return this;
    }

    public VolumeMountBuilder<ContainerBuilder<TParent>> AddVolumeMount()
    {
        _volumeMountBuilders ??= new List<IBuilder<VolumeMount>>();
        VolumeMountBuilder<ContainerBuilder<TParent>> builder = new(this);
        _volumeMountBuilders.Add(builder);
        return builder;
    }

    public ContainerBuilder<TParent> AddVolumeMount(string name, string mountPath)
    {
        AddVolumeMount()
            .SetName(name)
            .SetMountPath(mountPath);
        return this;
    }

    public ContainerEnvBuilder<TParent> AddEnv()
    {
        _envBuilders ??= new List<IBuilder<ContainerEnv>>();
        ContainerEnvBuilder<TParent> builder = new(this);
        _envBuilders.Add(builder);
        return builder;
    }

    public ContainerBuilder<TParent> AddEnv(string name, string value)
    {
        AddEnv()
            .SetName(name)
            .SetValue(value);
        return this;
    }

    public ContainerSecurityContextBuilder<ContainerBuilder<TParent>> AddSecurityContext()
    {
        ContainerSecurityContextBuilder<ContainerBuilder<TParent>> builder = new(this);
        _securityContextBuilder = builder;
        return builder;
    }

    public ContainerResourcesBuilder<ContainerBuilder<TParent>> AddResources()
    {
        ContainerResourcesBuilder<ContainerBuilder<TParent>> builder = new(this);
        _containerResourcesBuilder = builder;
        return builder;
    }

    public virtual TemplateContainer Build()
    {
        if (_image == null)
            throw new InvalidOperationException("Image must be set");

        return new TemplateContainer(
            _image,
            _command,
            _arguments,
            _workingDirectory,
            _volumeMountBuilders?.Select(x => x.Build()),
            _envBuilders?.Select(x => x.Build()),
            _securityContextBuilder?.Build(),
            _containerResourcesBuilder?.Build()
        );
    }
}