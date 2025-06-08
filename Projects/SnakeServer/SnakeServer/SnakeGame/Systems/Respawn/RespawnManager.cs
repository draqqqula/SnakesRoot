using ServerEngine.Interfaces;
using ServerEngine.Interfaces.Services;
using ServerEngine.Models;
using SnakeGame.Mechanics.Bodies;
using SnakeGame.Models.Input.Internal;
using SnakeGame.Services;
using SnakeGame.Services.Output.Commands;
using SnakeGame.Systems.EventPipeline;
using SnakeGame.Systems.GameObjects.Characters;
using SnakeGame.Systems.Respawn;
using SnakeGame.Systems.Timer;
using SnakeGame.Systems.ViewPort.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Mechanics.Respawn;

internal class RespawnManager(

    SnakeSpawner Spawner,
    IGameConfiguration Configuration,
    IEnumerable<IAbilityFactory> Factories,
    CommandSender Sender,
    IViewPortBinder Binder,
    ITimerScheduler Timer

    ) : ISessionService, IInputService<OptionInput>, IUpdateService, IEventSource<RespawnEventArgs>
{
    private readonly TimeSpan RespawnTime = Configuration.Get<TimeSpan>(nameof(RespawnTime));

    private readonly TimeSpan ShowKillerDelay = Configuration.Get<TimeSpan>(nameof(ShowKillerDelay));

    private readonly TimeSpan ShowRespawnMenuDelay = Configuration.Get<TimeSpan>(nameof(ShowRespawnMenuDelay));

    private readonly Dictionary<ClientIdentifier, IAbilityFactory> _selectedAbilities = [];

    private readonly Dictionary<ClientIdentifier, RespawnTimer> _awaitingInput = [];

    private readonly IAbilityFactory[] _availableAbilities = Factories.ToArray();

    public event Action<RespawnEventArgs> Fire;

    public void QueueRespawn(ClientIdentifier id, TransformBase? temporaryTarget, bool showRespawnMenuImmediately)
    {
        Binder.Reset(id);
        if (temporaryTarget is not null)
        {
            Timer.Set(ShowKillerDelay, () =>
            {
                if (!temporaryTarget.IsDisposed)
                {
                    Binder.Bind(id, temporaryTarget);
                }
            });
        }
        var timer = RespawnTimer.Set(RespawnTime);
        _awaitingInput.Add(id, timer);
        if (showRespawnMenuImmediately)
        {
            ShowRespawnMenuCommand.To(id, Sender, RespawnTime);
        }
        else
        {
            Timer.Set(ShowRespawnMenuDelay, () => ShowRespawnMenuCommand.To(id, Sender, RespawnTime - ShowRespawnMenuDelay));
        }
    }

    private void Respawn(ClientIdentifier id)
    {
        var character = Spawner.Spawn(id, _selectedAbilities[id]);
        Binder.Bind(id, character.Transform);
        Fire?.Invoke(new RespawnEventArgs()
        {
            Character = character,
            Client = id
        });
    }

    public void OnInput(ClientIdentifier id, OptionInput data)
    {
        if (_awaitingInput.ContainsKey(id))
        {
            if (data.OptionIndex < 0 || data.OptionIndex >= _availableAbilities.Length)
            {
                return;
            }
            _selectedAbilities[id] = _availableAbilities[data.OptionIndex];
        }
    }

    public void OnJoin(IGameContext context, ClientIdentifier id)
    {
        QueueRespawn(id, null, true);
        _selectedAbilities.Add(id, _availableAbilities.First());
    }

    public void OnLeave(IGameContext context, ClientIdentifier id)
    {
        Binder.Reset(id);
        _awaitingInput.Remove(id);
        _selectedAbilities.Remove(id);
    }

    public void Update(IGameContext context)
    {
        foreach (var timer in _awaitingInput.ToArray())
        {
            if (timer.Value.Expired)
            {
                Respawn(timer.Key);
                _awaitingInput.Remove(timer.Key);
            }
        }
    }
}
