using ServerEngine.Profiling.Interfaces;
using SessionApi.Services.Profiling;
using SessionApi.Services.Profiling.Interfaces;

namespace SessionApi.Services;

public static class StartUp
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<ProfilingManager>();
        services.AddScoped<IProfileAggregator>(provider => provider.GetRequiredService<ProfilingManager>());
        services.AddScoped<IProfileRecorder>(provider => provider.GetRequiredService<ProfilingManager>());
        services.AddScoped<IProfileLogger, ConsoleProfileLogger>();
    }
}
