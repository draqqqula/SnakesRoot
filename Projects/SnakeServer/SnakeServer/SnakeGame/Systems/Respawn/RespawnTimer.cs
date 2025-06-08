namespace SnakeGame.Systems.Respawn;

internal record RespawnTimer
{
    public required DateTime Expires { get; init; }

    public bool Expired => DateTime.UtcNow >= Expires;

    public static RespawnTimer Set(TimeSpan duration)
    {
        return new RespawnTimer()
        {
            Expires = DateTime.UtcNow + duration,
        };
    }
}
