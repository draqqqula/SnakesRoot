using MatchMakingLogic.Party.Interfaces;
using MatchMakingLogic.Party.Models;
using MatchMakingService.Services.Interfaces;
using System.Net.WebSockets;
using System.Text;

namespace MatchMakingService.Services;

public class WebSocketPartyMemberDialogue(IPartyManager manager, IHttpContextAccessor accessor) : IPartyMemberDialogue
{
    private const int IdSize = 16;
    private const int DataSize = 1;
    private static byte[] GetBytes(IEnumerable<(Client, bool)> members)
    {
        var arr = members.ToArray();
        var buffer = new byte[arr.Length * (IdSize + DataSize)];
        for (int i = 0; i < arr.Length; i++)
        {
            buffer[i] = Convert.ToByte(arr[i].Item2);
            arr[i].Item1.Id.ToByteArray().CopyTo(buffer, i * (IdSize + DataSize) + DataSize);
        }
        return buffer;
    }

    public async Task StartAsync(PartyCode code, Client member)
    {
        if (accessor.HttpContext is null)
        {
            return;
        }

        var party = manager.TryGetByCode(code);

        if (party is null)
        {
            accessor.HttpContext.Response.StatusCode = 400;
            return;
        }
        using var ws = await accessor.HttpContext.WebSockets.AcceptWebSocketAsync();

        await ws.SendAsync(GetBytes(party.Ready), WebSocketMessageType.Binary, true, CancellationToken.None);

        party.AddMember(member);

        var cts = new CancellationTokenSource();

        using var binder = new WebSocketPartyEventBinder(ws, party.EventSource, cts.Token).Binded();

        var buffer = new byte[1];

        while (!ws.CloseStatus.HasValue)
        {
            var response = await ws.ReceiveAsync(buffer, cts.Token);
            if (response.MessageType == WebSocketMessageType.Close)
            {
                break;
            }
            else if (buffer[0] == 1)
            {
                party.ToggleReady(member);
            }
        }
        binder.Dispose();
        party.RemoveMember(member);
    }
}
