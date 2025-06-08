using Microsoft.Extensions.DependencyInjection;
using ServerEngine.Interfaces.Services;
using SnakeGameAssets.Services;
using SnakeGameAssets.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGameAssets;

public static class StartUp
{
    public static void AddAssets(this IServiceCollection services)
    {
        services.AddSingleton<PickupPatternContainer>();
        services.AddSingleton<IPickupPatternContainer>(provider => provider.GetRequiredService<PickupPatternContainer>());
        services.AddSingleton<IStartUpService>(provider => provider.GetRequiredService<PickupPatternContainer>());
        services.AddSingleton<IMapLoader, MapLoader>();
    }
}
