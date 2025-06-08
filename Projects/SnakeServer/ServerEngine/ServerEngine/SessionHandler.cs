using ServerEngine.Interfaces.Output;
using ServerEngine.Models;
using ServerEngine.Models.Input;
using ServerEngine.Models.Output;
using System.Collections.Concurrent;
using System.ComponentModel;

namespace ServerEngine;

internal class SessionHandler
{
    internal volatile int PlayerCounter = 0;

    internal volatile int TickCounter = 0;

    internal volatile bool Closed = false;

    internal volatile float TimeScale = 1.0f;

    internal List<OutputMessage> Output { get; } = [];
    internal SemaphoreSlim Semaphore { get; } = new SemaphoreSlim(1);
    internal ConcurrentQueue<ClientIdentifier> JoinQueue { get; } = new ();
    internal ConcurrentQueue<ClientIdentifier> LeaveQueue { get; } = new ();
    internal ConcurrentQueue<ClientInput> InputQueue { get; } = new ();
}
