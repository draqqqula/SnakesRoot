namespace SnakeGame.Systems.EventPipeline;

public interface IEventListener<T>
{
    public void OnFired(T arguments);
}
