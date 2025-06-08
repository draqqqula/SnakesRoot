using ServerEngine.Interfaces;
using ServerEngine.Interfaces.Services;
using ServerEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Systems.Digging;

internal class MapPictureRepository : ISessionService
{
    private readonly Dictionary<ClientIdentifier, ClientMapPicture> _pictures = [];
    private readonly List<int> _globalDiff = [];
    public IReadOnlyDictionary<ClientIdentifier, ClientMapPicture> Pictures => _pictures;

    public void Update(IEnumerable<int> broken)
    {
        _globalDiff.AddRange(broken);
    }

    public void OnJoin(IGameContext context, ClientIdentifier id)
    {
        _pictures.Add(id, new ClientMapPicture(_globalDiff));
    }

    public void OnLeave(IGameContext context, ClientIdentifier id)
    {
        _pictures.Remove(id);
    }
}
