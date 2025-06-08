using Microsoft.Extensions.DependencyInjection;
using ServerEngine.Interfaces.Output;

namespace ServerEngine.Interfaces;

public interface ISessionLauncher
{
    public void Prepare(IServiceCollection services);
}

public static class LauncherExtensions
{
    public static void AddOutputHandlerScoped<TCollect,TProvide>(this IServiceCollection services)
    {
        services.AddScoped<OutputHandler, OutputHandler<TCollect,TProvide>>();
    }

    public static void AddOutputFabricScoped<TCollect, TProvide, TTransformer>(this IServiceCollection services) where TTransformer : class, IOutputCollector<TCollect>, IOutputProvider<TProvide>
    {
        services.AddScoped<TTransformer>();
        services.AddScoped<IOutputProvider<TProvide>>(provider => provider.GetRequiredService<TTransformer>());
        services.AddScoped<IOutputCollector<TCollect>>(provider => provider.GetRequiredService<TTransformer>());
        services.AddOutputHandlerScoped<TCollect, TProvide>();
    }

    public static void AddOutputHandlerSingleton<TCollect, TProvide>(this IServiceCollection services)
    {
        services.AddSingleton<OutputHandler, OutputHandler<TCollect, TProvide>>();
    }

    public static void AddOutputFabricSingleton<TCollect, TProvide, TTransformer>(this IServiceCollection services) where TTransformer : class, IOutputCollector<TCollect>, IOutputProvider<TProvide>
    {
        services.AddSingleton<TTransformer>();
        services.AddSingleton<IOutputProvider<TProvide>>(provider => provider.GetRequiredService<TTransformer>());
        services.AddSingleton<IOutputCollector<TCollect>>(provider => provider.GetRequiredService<TTransformer>());
        services.AddOutputHandlerSingleton<TCollect, TProvide>();
    }

    public static void AddOutputHandlerSingleton<TProvide>(this IServiceCollection services)
    {
        services.AddSingleton<OutputHandler, OutputHandler<TProvide>>();
    }
}