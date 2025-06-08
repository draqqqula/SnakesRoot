namespace ServerEngine.Routine;

public interface IRoutine
{
    public bool IsReady(IServiceProvider provider);
}
