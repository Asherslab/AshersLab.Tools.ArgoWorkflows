using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AshersLab.Tools.ArgoWorkflows.Extensions;

public static class HostBuilderExtensions
{
    public static IHostBuilder AddConfigService<T>(this IHostBuilder builder, string name, Action<T>? configure = null) where T : class
    {
        return builder.ConfigureServices((ctx, services) =>
        {
            T config = ctx.Configuration.GetSection(name).Get<T>();
            configure?.Invoke(config);
            services.AddSingleton(config);
        });
    }
}