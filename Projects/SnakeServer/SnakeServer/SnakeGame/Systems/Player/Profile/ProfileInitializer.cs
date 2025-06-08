using Microsoft.Extensions.DependencyInjection;
using ServerEngine;
using ServerEngine.Interfaces;
using ServerEngine.Interfaces.Services;
using ServerEngine.Models;
using SnakeGame.Common;
using SnakeGame.Models.Input.External;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Systems.Player.Profile;

internal class ProfileInitializer(IServiceProvider provider) : IInputService<IntroductionInput>
{
    private readonly IDictionary<Guid, string> _idToNickname =
        provider.GetRequiredKeyedService<IDictionary<Guid, string>>("nicknames");
    private readonly IDictionary<Guid, TeamColor> _idToTeam =
        provider.GetRequiredKeyedService<IDictionary<Guid, TeamColor>>("teams");

    public void OnInput(ClientIdentifier client, IntroductionInput data)
    {
        _idToNickname[client.Id] = data.Nickname;
    }
}
