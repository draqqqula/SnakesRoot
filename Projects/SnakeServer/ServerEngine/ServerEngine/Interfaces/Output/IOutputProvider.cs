namespace ServerEngine.Interfaces.Output;

public interface IOutputProvider<out T>
{
    public T Get();
}
