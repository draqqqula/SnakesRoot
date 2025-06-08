using Microsoft.Extensions.DependencyInjection;
using ServerEngine.Interfaces.Serialization;
using ServerEngine.Interfaces.Services;
using SnakeGame.Common;
using SnakeGame.Services.Output;
using SnakeGame.Systems.RuntimeCommands.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Systems.Serializers;

internal static class StartUp
{
    public static void AddCommonSerializers(this IServiceCollection services)
    {
        services.AddSingleton<IBinarySerializer<float>, FloatSerializer>();
        services.AddSingleton<IBinarySerializer<string>, StringSerializer>();
        services.AddSingleton<IBinarySerializer<int>, IntSerializer>();
        services.AddSingleton<IBinarySerializer<Vector2>, Vector2Serializer>();
        services.AddSingleton<IBinarySerializer<int[]>, IntArraySerializer>();
        services.AddSingleton<IBinarySerializer<Guid>, GuidSerializer>();
        services.AddSingleton<IBinarySerializer<TeamColor>, TeamColorSerializer>();
        services.AddSingleton<IBinarySerializer<bool>, BooleanSerializer>();
    }
}
