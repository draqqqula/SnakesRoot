namespace ServerEngine.Interfaces.Serialization;

public interface IBinarySerializer<T>
{
    public void Serialize(BinaryWriter writer, T value);
}
