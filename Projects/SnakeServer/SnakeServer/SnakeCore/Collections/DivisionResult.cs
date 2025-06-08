namespace SnakeCore.Collections;

public readonly struct DivisionResult<T>
{
    public required T Include { get; init; }
    public required T Exclude { get; init; }
}
