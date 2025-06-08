using ServerEngine.Models;

namespace SessionApi.Models.Response;

public record StatusResponse
{
    public required string Status { get; init; }
    public required int Online { get; init; }
}
