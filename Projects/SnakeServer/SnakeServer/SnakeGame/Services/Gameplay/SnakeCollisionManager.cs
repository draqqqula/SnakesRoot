using ServerEngine.Interfaces;
using ServerEngine.Interfaces.Services;
using ServerEngine.Models;
using SnakeGame.Mechanics.Collision;
using SnakeGame.Mechanics.Collision.Shapes;
using SnakeGame.Mechanics.Frames;
using SnakeGame.Mechanics.Respawn;
using SnakeGame.Models.Gameplay;
using SnakeGame.Systems.EventPipeline;
using SnakeGame.Systems.GameObjects.Characters;
using SnakeGame.Systems.Killfeed;
using SnakeGame.Systems.RuntimeCommands.Interfaces;
using SnakeGame.Systems.Statistics.Interfaces;
using System.Xml.Linq;

namespace SnakeGame.Services.Gameplay;

internal class SnakeCollisionManager
    (
    Dictionary<ClientIdentifier, SnakeCharacter> Players,
    ICollisionChecker collision,
    SnakeSpawner SnakeSpawner,
    RespawnManager RespawnManager,
    IStatisticsFactory Statistics
    ) : IUpdateService, IEventSource<KillInteraction>
{
    class Kill
    {
        public required SnakeCharacter Killer { get; init; }
        public required SnakeCharacter Victim { get; init; }
    }

    private readonly IStatistic<int> KillCount = Statistics.Declare(nameof(KillCount), 0);
    private readonly IStatistic<int> DeathCount = Statistics.Declare(nameof(DeathCount), 0);

    public event Action<KillInteraction> Fire;

    public void Update(IGameContext context)
    {
        var kills = new List<Kill>();
        foreach (var master in Players.Values)
        {
            foreach (var slave in Players.Values.Where(it => it.Team != master.Team))
            {
                kills.AddRange(Interact(context, master, slave));
            }
        }
        foreach (var kill in kills)
        {
            KillCount.Change(kill.Killer.ClientId, c => c + 1);
            DeathCount.Change(kill.Victim.ClientId, c => c + 1);
            SnakeSpawner.Despawn(context, kill.Victim.ClientId);
            RespawnManager.QueueRespawn(kill.Victim.ClientId, kill.Killer.Transform, false);
            Fire?.Invoke(new KillInteraction()
            {
                Killer = kill.Killer.ClientId,
                Victim = kill.Victim.ClientId,
            });
        }
    }

    private IEnumerable<Kill> Interact(IGameContext context, SnakeCharacter master, SnakeCharacter slave)
    {
        if (master.Body.Count == 0 || slave.Body.Count == 0)
        {
            yield break;
        }

        var masterTier = master.MaxTier;
        var slaveTier = slave.MaxTier;

        if (collision.IsColliding(master.Head, slave))
        {
            Console.WriteLine($"Head Collision: master - {master} slave - {slave}");

            if (masterTier >= slaveTier || master.OnBase)
            {
                Console.WriteLine($"Master tier({masterTier}) greater or equal slave tier({slaveTier})");
                CollapseBody(context, slave, 0);
                yield return new Kill()
                {
                    Killer = master,
                    Victim = slave,
                };
            }
            if (slaveTier >= masterTier || slave.OnBase)
            {
                Console.WriteLine($"Slave tier({masterTier}) greater or equal master tier({slaveTier})");
                CollapseBody(context, master, 0);
                yield return new Kill()
                {
                    Killer = slave,
                    Victim = master,
                };
            }
            Console.WriteLine($"Result: master - {master} slave - {slave}");
            yield break;
        }

        foreach (var (segment, index) in slave.Body.Select((i, it) => (i, it)))
        {
            if (!collision.IsColliding(master, segment.Item))
            {
                continue;
            }

            Console.WriteLine($"Segment collision: master - {master} slave - {slave} segment index {index}");

            var response = segment.Item.Contact(context, master);

            if (response.HasFlag(ContactResult.Killed))
            {
                Console.WriteLine($"masterTier less or equal segmentTier");
                CollapseBody(context, master, 0);
                yield return new Kill()
                {
                    Killer = slave,
                    Victim = master
                };
            }
            if (response.HasFlag(ContactResult.Consumed))
            {
                Console.WriteLine($"masterTier greater or equal segmentTier");
                CollapseBody(context, slave, index);
                if (slave.Body.Count == 0)
                {
                    yield return new Kill()
                    {
                        Killer = master,
                        Victim = slave
                    };
                }
            }
            Console.WriteLine($"Result: master - {master} slave - {slave}");
            yield break;
        }
    }

    private void CollapseBody(IGameContext context, SnakeCharacter character, int start)
    {
        if (start >= character.Body.Count)
        {
            return;
        }
        var collapsed = character.Body.Skip(start).ToArray();
        foreach ( var segment in collapsed)
        {
            segment.Item.Detach(context);
            character.Body.Remove(segment);
            segment.Item.Transform.Dispose();
        }
    }
}
