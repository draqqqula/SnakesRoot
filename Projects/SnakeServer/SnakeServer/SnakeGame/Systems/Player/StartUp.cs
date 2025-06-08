using Microsoft.Extensions.DependencyInjection;
using SnakeGame.Systems.Player.Origin;
using SnakeGame.Systems.Player.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Systems.Player;

internal static class StartUp
{
    public static void AddPlayer(this IServiceCollection services)
    {
        services.AddPlayerProfile();
        services.AddPlayerOrigin();
    }
}
