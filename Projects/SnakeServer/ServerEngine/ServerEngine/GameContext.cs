using Microsoft.Extensions.DependencyInjection;
using ServerEngine.Interfaces;

namespace ServerEngine;

internal class GameContext(IServiceProvider provider) : IGameContext
{
    private readonly IServiceProvider _provider = provider;

    public float DeltaTimeF { get; internal set; }

    public TimeSpan DeltaTime { get; internal set; }

    public T Using<T>()
    {
        return _provider.GetRequiredService<T>();
    }
}