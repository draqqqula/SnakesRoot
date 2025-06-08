using MatchMakingService.Services.Interfaces;

namespace MatchMakingService.Services;

public static class StartUp
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IPartyMemberDialogue, WebSocketPartyMemberDialogue>();
        services.AddScoped<IPartyLeaderDialogue, WebSocketPartyLeaderDialogue>();
        return services;
    }
}
