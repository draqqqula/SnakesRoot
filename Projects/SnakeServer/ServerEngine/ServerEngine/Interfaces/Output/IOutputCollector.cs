namespace ServerEngine.Interfaces.Output;

public interface IOutputCollector<in T>
{
    public void Pass(T data);
}
