using MessageSchemes;
using Microsoft.Extensions.DependencyInjection;
using ServerEngine.Interfaces;
using ServerEngine.Interfaces.Services;
using ServerEngine.Models;
using SnakeGame.Common;
using SnakeGame.Mechanics.Collision;
using SnakeGame.Mechanics.Frames;
using SnakeGame.Models.Gameplay;
using SnakeGame.Models.Output.Internal;
using SnakeGame.Services.Output.Commands;
using System.Numerics;
using System.Security.Claims;

namespace SnakeGame.Services.Gameplay;

internal class ScoreManager(
    Dictionary<TeamColor, TeamContext> Teams,
    CommandSender Sender,
    IServiceProvider ServiceProvider
    ) : IStartUpService, ISessionService
{
    private readonly IDictionary<TeamColor, int> Scores = ServiceProvider.GetRequiredKeyedService<IDictionary<TeamColor, int>>("Scores");
    public void Start(IGameContext context)
    {
        foreach ( var team in Teams )
        {
            Scores.Add(team.Key, 0);
            team.Value.ScoreChangedEvent += value => Broadcast(team.Key, value);
        }
    }

    private void Broadcast(TeamColor team, int newValue)
    {
        var clients = Teams.SelectMany(it => it.Value.Members);
        Scores[team] = newValue;
        foreach ( var client in clients )
        {
            ChangeScoreCommand.To(client, Sender, new TeamScore() { Color = team, Score = newValue });
        }
    }

    public void OnJoin(IGameContext context, ClientIdentifier id)
    {
        ChangeScoreCommand.To(id, Sender, Teams.Select(it => new TeamScore() 
        { 
            Score = it.Value.Score, 
            Color = it.Key 
        }).ToArray());
    }

    public void OnLeave(IGameContext context, ClientIdentifier id)
    {
    }
}
