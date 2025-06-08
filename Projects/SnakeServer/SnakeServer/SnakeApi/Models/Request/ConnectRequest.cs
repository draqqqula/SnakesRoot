using Microsoft.AspNetCore.Mvc;

namespace SessionApi.Models.Request;

public record ConnectRequest
{
    [FromHeader]
    public string Nickname { get; init; } = "Guest";
}
