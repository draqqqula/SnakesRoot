namespace SnakeGame.Services.Output;

internal interface ISerializableCommand
{
    public void Serialize(BinaryWriter writer);
}
