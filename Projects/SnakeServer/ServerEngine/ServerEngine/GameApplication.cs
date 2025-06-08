using Microsoft.Extensions.DependencyInjection;
using ServerEngine.Interfaces;
using ServerEngine.Models;
using ServerEngine.Profiling;
using ServerEngine.Profiling.Interfaces;
using System.Text;
using System.Text.Json;

namespace ServerEngine;

internal class GameApplication(IServiceProvider provider)
    : IGameApplication
{
    public async Task<ISessionManager> CreateSessionAsync(ISessionLauncher launcher)
    {
        var handler = new SessionHandler();
        var state = new GameState(handler, provider.GetRequiredService<IProfileRecorder>());

        state.Initialize(launcher);
        var manager = new SessionManager(handler, state, provider.GetRequiredService<IProfileAggregator>());

        return manager;
    }
}
