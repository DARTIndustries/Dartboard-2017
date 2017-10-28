using Common.Logging;

namespace DART.Dartboard.Utils
{
    public abstract class LoggableObject
    {
        protected ILog Log;

        protected LoggableObject()
        {
            Log = LogManager.GetLogger(GetType());
        }
    }
}
