using MatchMakingLogic.Party.Implementation;
using MatchMakingLogic.Party.Implementation.Generators;
using MatchMakingLogic.Party.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace MatchMakingLogic;

public static class StartUp
{
    public static IServiceCollection AddLogic(this IServiceCollection services)
    {
        services.AddSingleton<ICodeGenerator, WordGenerator>();
        services.AddSingleton<IPartyManager, PartyManager>();
        return services;
    }
}
