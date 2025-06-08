
using SnakeGame.Common;

namespace SnakeGame.Services.Output.Commands;

internal struct PersonalSummary
{
    public int MaxScore { get; init; }
    public int Scored { get; init; }
    public int Kills { get; init; }
    public int Deaths { get; init; }
}

internal struct TeamScore
{
    public required TeamColor Color { get; init; }
    public required int Score { get; init; }
}

internal class ScoreSummary
{
    public List<TeamScore> Results { get; } = [];
}

internal class MatchSummaryCommand : ISerializableCommand
{
    public required bool Winner { get; init; }
    public required PersonalSummary Personal { get; init; }
    public required ScoreSummary Scores { get; init; }
    public void Serialize(BinaryWriter writer)
    {
        writer.Write((byte)5);
        writer.Write(Winner);
        writer.Write(Personal.MaxScore);
        writer.Write(Personal.Kills);
        writer.Write(Personal.Deaths);
        writer.Write(Personal.Scored);
        writer.Write((byte)Scores.Results.Count);
        foreach (var result in Scores.Results)
        {
            writer.Write((byte)result.Color);
            writer.Write(result.Score);
        }
    }
}
