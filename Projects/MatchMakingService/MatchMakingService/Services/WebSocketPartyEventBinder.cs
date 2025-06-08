using MatchMakingLogic.Party.Interfaces;
using MatchMakingLogic.Party.Models;
using System.Net.WebSockets;

namespace MatchMakingService.Services;

public class WebSocketPartyEventBinder(WebSocket ws, IPartyEventSource source, CancellationToken cancellationToken) : IDisposable
{
    public WebSocketPartyEventBinder Binded()
    {
        source.OnPlayerJoined += NotifyJoinedAsync;
        source.OnPlayerLeft += NotifyLeftAsync;
        source.OnLeaderChanged += NotifyLeaderChangedAsync;
        source.OnSessionFound += NotifySessionFoundAsync;
        source.OnToggleReady += NotifyToggleReadyAsync;
        return this;
    }

    private async Task Send(byte signatureByte, byte[] data)
    {
        if (ws.CloseStatus.HasValue)
        {
            Dispose();
            return;
        }
        await ws.SendAsync(PrefixData(signatureByte, data), WebSocketMessageType.Binary, true, cancellationToken);
    }

    private byte[] PrefixData(byte signatureByte, byte[] data)
    {
        var destination = new byte[data.Length + 1];
        destination[0] = signatureByte;
        data.CopyTo(destination, 1);
        return destination;
    }

    private async void NotifyJoinedAsync(Client client)
    {
        await Send(1, client.Id.ToByteArray());
    }

    private async void NotifyLeftAsync(Client client)
    {
        await Send(2, client.Id.ToByteArray());
    }

    private async void NotifyLeaderChangedAsync(Client client)
    {
        await Send(3, client.Id.ToByteArray());
    }

    private async void NotifyToggleReadyAsync(Client client)
    {
        await Send(4, client.Id.ToByteArray());
    }

    private async void NotifySessionFoundAsync(Session session)
    {
        await Send(1, session.Id.ToByteArray());
    }

    public void Dispose()
    {
        source.OnPlayerJoined -= NotifyJoinedAsync;
        source.OnPlayerLeft -= NotifyLeftAsync;
        source.OnLeaderChanged -= NotifyLeaderChangedAsync;
        source.OnSessionFound -= NotifySessionFoundAsync;
        source.OnToggleReady -= NotifyToggleReadyAsync;
    }
}
