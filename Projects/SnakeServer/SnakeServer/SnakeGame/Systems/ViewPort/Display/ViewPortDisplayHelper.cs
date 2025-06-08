using ServerEngine.Interfaces;
using ServerEngine.Interfaces.Services;
using ServerEngine.Models;
using SnakeGame.Common;
using SnakeGame.Mechanics.Bodies;
using SnakeGame.Mechanics.Frames;
using SnakeGame.Models.Gameplay;
using SnakeGame.Services;
using SnakeGame.Services.Gameplay;
using SnakeGame.Services.Output;
using SnakeGame.Services.Output.Commands;

namespace SnakeGame.Mechanics.ViewPort.Display;

internal class ViewPortDisplayHelper(
    Dictionary<ClientIdentifier, ClientViewPort> ViewPorts, 
    FrameFactory Factory, 
    CommandSender Sender, 
    MinimapManager Minimap,
    Dictionary<TeamColor, TeamContext> Teams) : IUpdateService
{
    private readonly Dictionary<ClientViewPort, ViewDisplay> _displayObjects = [];

    public void Update(IGameContext context)
    {
        foreach (var viewPort in ViewPorts)
        {
            ViewDisplay display;
            if (_displayObjects.TryGetValue(viewPort.Value, out display))
            {
                display.Transform.Position = viewPort.Value.Transform.Position;
                display.Transform.Angle = viewPort.Value.Transform.Angle;
                display.Transform.Size = viewPort.Value.Transform.Size;
            }
            else
            {
                var team = Teams.FirstOrDefault(it => it.Value.Members.Contains(viewPort.Key));
                display = new ViewDisplay()
                {
                    Transform = Factory.Create($"viewport_{team.Key}", viewPort.Value.Transform.ReadOnly),
                };
                AttachCameraCommand.To(viewPort.Key, Sender, display.Transform.Id.Value);
                Minimap.Pin(viewPort.Key, Sender, display.Transform.Id.Value);

                PinTeamAreas(viewPort.Key);
                ExchangeWithTeam(viewPort.Key, display);

                _displayObjects.Add(viewPort.Value, display);
            }
        }
    }

    private void PinTeamAreas(ClientIdentifier id)
    {
        foreach(var team in Teams.Values)
        {
            Minimap.Pin(id, Sender, team.Area.Transform.Id.Value);
        }
    }

    private void ExchangeWithTeam(ClientIdentifier clientId, ViewDisplay targetDisplay)
    {
        var team = Teams.FirstOrDefault(it => it.Value.Members.Contains(clientId));

        if (team.Value is null || team.Value.Members.Count == 1)
        {
            return;
        }

        foreach (var memberId in team.Value.Members.Except([clientId]))
        {
            Minimap.Pin(memberId, Sender, targetDisplay.Transform.Id.Value);

            if (ViewPorts.TryGetValue(memberId, out var vp) &&
                _displayObjects.TryGetValue(vp, out var memberDisplay))
            {
                Minimap.Pin(clientId, Sender, memberDisplay.Transform.Id.Value);
            }
        }
    }
}
