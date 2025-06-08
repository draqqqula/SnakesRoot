using MatchMakingLogic.Party.Interfaces;
using MatchMakingLogic.Party.Models;
using MatchMakingService.Services.Interfaces;
using System.Net.WebSockets;
using System.Text;

namespace MatchMakingService.Services;

public class WebSocketPartyLeaderDialogue(IPartyManager manager, IHttpContextAccessor accessor) : IPartyLeaderDialogue
{
    public async Task StartAsync(Client leader)
    {
        if (accessor.HttpContext?.WebSockets.IsWebSocketRequest ?? false)
        {
            using var ws = await accessor.HttpContext.WebSockets.AcceptWebSocketAsync();

            var code = manager.CreateFor(leader);
            var codeBytes = Encoding.UTF8.GetBytes(code.String);
            await ws.SendAsync(codeBytes, WebSocketMessageType.Text, true, CancellationToken.None);

            var party = manager.TryGetByCode(code);
            if (party is null)
            {
                return;
            }

            using var binder = new WebSocketPartyEventBinder(ws, party.EventSource, CancellationToken.None).Binded();

            var buffer = new byte[1];


            while (!ws.CloseStatus.HasValue)
            {
                var response = await ws.ReceiveAsync(buffer, CancellationToken.None);
                if (response.MessageType == WebSocketMessageType.Close)
                {
                    break;
                }
                party.ToggleReady(leader);
            }
            binder.Dispose();
            party.RemoveMember(leader);
        }
    }
}
