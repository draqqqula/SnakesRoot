using Microsoft.Extensions.DependencyInjection;
using ServerEngine.Interfaces.Services;
using ServerEngine.Models;
using SnakeGame.Mechanics.Frames.Output.Interfaces;
using SnakeGame.Mechanics.ViewPort;
using SnakeGame.Models.Output.External;
using SnakeGame.Services.Output;

namespace SnakeGame.Mechanics.ViewPort.Display;

internal static class StartUp
{
    public static void AddViewHelp(this IServiceCollection services)
    {
        services.AddSingleton<ViewPortDisplayHelper>();
        services.AddSingleton<IUpdateService>(provider => provider.GetRequiredService<ViewPortDisplayHelper>());
    }
}
