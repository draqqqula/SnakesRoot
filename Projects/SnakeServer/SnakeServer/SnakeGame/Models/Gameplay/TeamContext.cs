using ServerEngine.Models;

namespace SnakeGame.Models.Gameplay;

internal class TeamContext(TeamArea area)
{
    private int _score;
    public event Action<int> ScoreChangedEvent = delegate { };
    public int Score
    {
        get
        {
            return _score;
        }
        set
        {
            if (_score != value)
            {
                _score = value;
                ScoreChangedEvent.Invoke(value);
            }
        }
    }
    public List<ClientIdentifier> Members { get; } = [];
    public TeamArea Area { get; set; } = area;
    public HashSet<string> PowerUps { get; init; } = [];
}
