using ServerEngine.Interfaces;

namespace ServerEngine;

internal class InternalSessionController(SessionHandler Handler) : IInternalSessionController
{
    public void Finish()
    {
        Handler.Closed = true;
    }

    public void SetTimeScale(float scale)
    {
        Handler.TimeScale = scale;
    }
}
