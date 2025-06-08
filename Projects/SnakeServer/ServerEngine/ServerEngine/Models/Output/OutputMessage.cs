using ServerEngine.Interfaces.Output;

namespace ServerEngine.Models.Output;

internal abstract class OutputMessage
{
    public abstract IOutputProvider<T>? TryGet<T>();
}

internal class OutputMessage<TData>(IOutputProvider<TData> reader) : OutputMessage
{
    private readonly IOutputProvider<TData> _reader = reader;
    public override IOutputProvider<T>? TryGet<T>() where T : default
    {
        if (_reader is IOutputProvider<T> required)
        {
            return required;
        }
        return default;
    }
}
