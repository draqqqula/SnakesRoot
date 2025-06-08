namespace ServerEngine.Models;

public record ClientIdentifier
{
    public Guid Id { get; init; } = Guid.NewGuid();
}
