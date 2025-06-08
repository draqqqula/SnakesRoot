using Microsoft.Extensions.DependencyInjection;
using ServerEngine.Interfaces.Services;
using SnakeGame.Common;
using SnakeGame.Models.Input.External;
using SnakeGame.Systems.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Systems.Player.Profile;

public static class StartUp
{
    public static void AddPlayerProfile(this IServiceCollection services)
    {
        services.AddSyncronizedDictionary<Guid, string>("nicknames");
        services.AddSyncronizedDictionary<Guid, TeamColor>("teams");
        services.AddSingleton<IInputService<IntroductionInput>, ProfileInitializer>();
    }
}
