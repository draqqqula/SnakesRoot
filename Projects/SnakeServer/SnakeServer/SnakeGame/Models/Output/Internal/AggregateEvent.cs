namespace SnakeGame.Models.Output.Internal;

internal struct AggregateEvent
{
    public required byte PoolId { get; init; }
    public required ushort EntityId { get; init; }
    public required byte FieldId { get; init; }
    public required object Data { get; init; }
}
