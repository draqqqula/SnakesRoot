namespace SnakeGame;

public enum GameMode
{
    Dual,
    Quad
}

public class MatchConfiguration
{
    public required GameMode Mode { get; init; }
    public required int TeamSize { get; init; }
    public required TimeSpan Duration { get; init; }
    public required int ScoreToWin { get; init; }
}
