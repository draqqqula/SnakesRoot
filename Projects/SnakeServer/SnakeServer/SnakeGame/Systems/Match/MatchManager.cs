using Microsoft.Extensions.DependencyInjection;
using ServerEngine.Interfaces;
using ServerEngine.Interfaces.Services;
using ServerEngine.Models;
using SnakeGame.Common;
using SnakeGame.Mechanics.Bodies;
using SnakeGame.Mechanics.Frames;
using SnakeGame.Models.Gameplay;
using SnakeGame.Services;
using SnakeGame.Services.Output.Commands;
using SnakeGame.Systems.RuntimeCommands;
using SnakeGame.Systems.RuntimeCommands.Interfaces;
using System.Numerics;

namespace SnakeGame.Systems.Match;

internal class MatchManager(
    MatchConfiguration Configuration,
    Dictionary<TeamColor, TeamContext> Teams,
    FrameFactory Factory,
    CommandSender Sender,
    IInternalSessionController InternalController,
    IRuntimeCommandFactory RuntimeCommandFactory,
    IServiceProvider provider
    ) :
    IUpdateService, IStartUpService, ISessionService
{
    private const float AreaDistance = 200;
    private readonly Vector2[] Locations =
        [
            new Vector2(1, 1) * AreaDistance,
            new Vector2(-1, -1) * AreaDistance,
            new Vector2(1, -1) * AreaDistance,
            new Vector2(-1, 1) * AreaDistance
        ];


    private const float GraceTime = 5;

    private IDictionary<Guid, TeamColor> _idToTeam = provider.GetRequiredKeyedService<IDictionary<Guid, TeamColor>>("teams");
    private RuntimeCommand<int> _showMatchResultsCommand = new RuntimeCommand<int>("ShowMatchResults", RuntimeCommandFactory);
    private bool OnGrace { get; set; } = false;
    private bool MatchEnded { get; set; } = false;
    private TimeSpan Timer { get; set; } = TimeSpan.Zero;

    private void AddTeams(params TeamColor[] colors)
    {
        foreach (var (color, index) in colors.Select((it, i) => (it, i)))
        {
            var area = new TeamArea()
            {
                Transform = Factory.Create($"area_{color}", new Transform()
                {
                    Angle = 0f,
                    Position = Locations[index],
                    Size = Vector2.One * 40f
                }),
            };
            Teams.Add(color, new TeamContext(area));
        }
    }

    public void Start(IGameContext context)
    {
        Timer = Configuration.Duration;
        if (Configuration.Mode == GameMode.Dual)
        {
            AddTeams(TeamColor.Red, TeamColor.Blue);
        }
        else if (Configuration.Mode == GameMode.Quad)
        {
            AddTeams(TeamColor.Red, TeamColor.Blue, TeamColor.Yellow, TeamColor.Green);
        }
    }

    public void Update(IGameContext context)
    {
        Timer -= context.DeltaTime;

        if (Timer <= TimeSpan.Zero)
        {
            if (OnGrace)
            {
                MatchEnded = true;
                InternalController.Finish();
            }
            else
            {
                InternalController.SetTimeScale(0.5f);
                SendResults(Teams.Values.OrderByDescending(it => it.Score).ToArray());
                Timer = TimeSpan.FromSeconds(GraceTime);
                OnGrace = true;
            }
        }
    }

    public void OnJoin(IGameContext context, ClientIdentifier id)
    {
        var (teamColor, team) = Teams
            .Where(it => it.Value.Members.Count < Configuration.TeamSize)
            .OrderBy(it => it.Value.Members.Count)
            .FirstOrDefault();
        if (team is null)
        {
            return;
        }
        team.Members.Add(id);
        _idToTeam[id.Id] = teamColor;
        UpdateTimerCommand.To(id, Sender, Timer);
    }

    public void OnLeave(IGameContext context, ClientIdentifier id)
    {
        if (_idToTeam.TryGetValue(id.Id, out var team))
        {
            Teams[team].Members.Remove(id);
        }
        _idToTeam.Remove(id.Id);
    }

    private void SendResults(TeamContext[] teamsOrdered)
    {
        for (int place = 0; place < teamsOrdered.Length; place++)
        {
            var team = teamsOrdered[place];
            foreach (var client in team.Members)
            {
                _showMatchResultsCommand.Send(client, place + 1);
            }
        }
    }
}
