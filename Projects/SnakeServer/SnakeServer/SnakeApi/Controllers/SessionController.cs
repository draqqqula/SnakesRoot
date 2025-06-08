using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ServerEngine.Interfaces;
using ServerEngine.Models;
using SessionApi.Filters;
using SessionApi.Models.Request;
using SessionApi.Models.Response;
using SnakeGame.Models.Input.External;
using SnakeGame.Models.Output.External;
using System;
using System.Net.WebSockets;

namespace WebSocketsSample.Controllers;

[Route("sessions")]
public class SessionController : ControllerBase
{
    private ISessionStorage<Guid> _storage;
    public SessionController(ISessionStorage<Guid> storage)
    {
        _storage = storage;
    }

    [HttpGet]
    [Route("status")]
    [ProducesResponseType(typeof(StatusResponse), 200)]
    public async Task<IActionResult> GetStatusAsync(
        [FromQuery] Guid sessionId)
    {
        var session = _storage.GetById(sessionId);
        if (session is null)
        {
            return Ok(new StatusResponse()
            {
                Status = "Not Found",
                Online = 0,
            });
        }
        return Ok(new StatusResponse()
        {
            Status = session.GetStatus().ToString(),
            Online = session.ConnectedCount,
        });
    }

    [Route("join")]
    public async Task ConnectAsync(
        [FromQuery] Guid sessionId,
        [FromHeader] ConnectRequest connectRequest)
    {
        var session = _storage.GetById(sessionId);
        if (HttpContext.WebSockets.IsWebSocketRequest && 
            session is not null && 
            session.GetStatus() == SessionStatus.Running)
        {
            using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();

            await StartCommunicationAsync(webSocket, session, connectRequest);
        }
        else
        {
            HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        }
    }

    private static async Task StartCommunicationAsync(WebSocket webSocket, ISessionManager session, ConnectRequest request)
    {
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        var id = new ClientIdentifier();
        var connection = await session.ConnectAsync(id);

        connection.SendInput(new IntroductionInput()
        {
            Nickname = request.Nickname,
        });

        await webSocket.SendAsync(
        new ArraySegment<byte>(id.Id.ToByteArray(), 0, 16),
        WebSocketMessageType.Binary,
        true,
        cancellationTokenSource.Token);

        var recieveTask = Task.Run(() => RecieveInLoopAsync(connection, webSocket, cancellationTokenSource));
        await SendInLoopAsync<ViewPortBasedBinaryOutput>(connection, webSocket, it => it[id], cancellationTokenSource);
    }

    private static async Task RecieveInLoopAsync(ISessionConnection connection, WebSocket webSocket,
        CancellationTokenSource cts)
    {
        var buffer = new byte[128];
        var receiveResult = await webSocket.ReceiveAsync(
            new ArraySegment<byte>(buffer), CancellationToken.None);
        HandleInputData(connection, buffer);

        while (!receiveResult.CloseStatus.HasValue && !connection.Closed)
        {
            buffer = new byte[128];
            if (receiveResult.MessageType == WebSocketMessageType.Close)
            {
                connection.Dispose();
                return;
            }
            receiveResult = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), cts.Token);
            HandleInputData(connection, buffer);
        }
        if (!connection.Closed)
        {
            connection.Dispose();
        }
    }

    private static void HandleInputData(ISessionConnection connection, byte[] buffer)
    {
        var stream = new MemoryStream(buffer);
        var reader = new BinaryReader(stream);
        connection.SendInput(new BinaryInput() { Data = reader });
    }

    private static async Task SendInLoopAsync<T>(ISessionConnection connection, WebSocket webSocket,
        Func<T, byte[]> func,
        CancellationTokenSource cts)
    {
        while (!webSocket.CloseStatus.HasValue && !connection.Closed)
        {
            var output = await connection.GetOutputAsync<T>();
            if (output is null)
            {
                continue;
            }
            try
            {
                var buffer = func(output);
                if (buffer.Length == 0)
                {
                    continue;
                }
                await webSocket.SendAsync(
                new ArraySegment<byte>(buffer, 0, buffer.Length),
                WebSocketMessageType.Binary,
                true,
                cts.Token);
            }
            catch (WebSocketException ex)
            {
                cts.Cancel();
                break;
            }
        }
        if (!connection.Closed)
        {
            connection.Dispose();
        }
    }

    private static async Task SendInitialAsync<T>(ISessionConnection connection, WebSocket webSocket,
        Func<T, byte[]> func,
        CancellationTokenSource cts)
    {
        var output = await connection.GetOutputAsync<T>();
        if (output is null)
        {
            return;
        }
        try
        {
            var buffer = func(output);
            if (buffer.Length == 0)
            {
                return;
            }
            await webSocket.SendAsync(
            new ArraySegment<byte>(buffer, 0, buffer.Length),
            WebSocketMessageType.Binary,
            true,
            cts.Token);
        }
        catch (WebSocketException ex)
        {
            cts.Cancel();
            connection.Dispose();
        }
    }
}