namespace AshersLab.Tools.ArgoWorkflows.Kubernetes.Extensions;

public static class StringExtensions
{
    public static string ArgoNormalize(this string value)
    {
        return value
            .ToLowerInvariant()
            .Replace(".", "-")
            .Replace(" ", "-")
            .Replace("/", "-");
    }
}