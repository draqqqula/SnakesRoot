using Microsoft.Extensions.DependencyInjection;
using ServerEngine.Interfaces;
using ServerEngine.Interfaces.Services;

namespace ServerEngine.Models.Input;

public abstract record ClientInput
{
    public required ClientIdentifier ClientId { get; init; }

    internal abstract void Broadcast(IServiceProvider services);
}

public record ClientInput<T> : ClientInput
{
    public required T Data { get; init; }

    internal override void Broadcast(IServiceProvider services)
    {
        TryFormat(services);
        TryHandle(services);
    }

    private void TryFormat(IServiceProvider services)
    {
        var formatters = services.GetServices<IInputFormatter<T>>();

        if (!formatters.Any())
        {
            return;
        }

        while (true)
        {
            var isResolved = false;
            foreach (var formatter in formatters)
            {
                var result = formatter.TryResolve(Data, ClientId);
                switch (result)
                {
                    case ResolveResult.Success: return;
                    case ResolveResult.Error: return;
                    case ResolveResult.Proceed: isResolved = true; break;
                }
            }
            if (!isResolved)
            {
                break;
            }
        }
    }

    private void TryHandle(IServiceProvider services)
    {
        var handlers = services.GetServices<IInputService<T>>();

        foreach (var handler in handlers)
        {
            handler.OnInput(ClientId, Data);
        }
    }
}
