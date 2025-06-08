using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ServerEngine.Interfaces;
using SessionApi.Filters;
using SessionApi.Services.Profiling.Interfaces;

namespace SessionApi.Controllers;

public class LaunchController : ControllerBase
{
    private readonly ISessionStorage<Guid> _storage;
    private readonly ISessionLauncher _launcher;
    private readonly IProfileLogger _profileLogger;
    public LaunchController(ISessionStorage<Guid> storage, ISessionLauncher launcher, IProfileLogger profileLogger)
    {
        _storage = storage;
        _launcher = launcher;
        _profileLogger = profileLogger;
    }

    [HttpGet]
    [Route("launch")]
    public async Task<IActionResult> LaunchAsync(
        [FromServices] IGameApplication game)
    {
        var session = await game.CreateSessionAsync(_launcher);
        var id = _storage.Add(session);
        session.OnClosed += () =>
        { 
            _storage.Remove(id);
            _profileLogger.WriteSnapshot();
        };
        return Ok(id);
    }

    [HttpGet]
    [Route("all")]
    public async Task<IActionResult> GetActiveAsync(
    [FromServices] IGameApplication game)
    {
        var collection = _storage.GetAll().ToArray();
        return Ok(collection);
    }

    [HttpGet]
    [Route("echo")]
    public async Task<IActionResult> EchoAsync(
        [FromQuery] Guid id
        )
    {
        return Ok(id);
    }
}
