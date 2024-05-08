using System.Diagnostics;

namespace Infrastructure.Services.Logging
{
    // not interface to let [Conditional()] works, but technically is interface
    public abstract class IConditionalLoggingService
    {
        [Conditional("DEV")]
        public void Log(string text, LogTag tag)
        {
            InternalLog(text, tag);
        }

        [Conditional("DEV")]
        public void LogWarning(string text, LogTag tag)
        {
            InternalLogWarning(text, tag);
        }

        [Conditional("DEV")]
        public void LogError(string text, LogTag tag)
        {
            InternalLogError(text, tag);
        }

        protected abstract void InternalLog(string text, LogTag tag);

        protected abstract void InternalLogWarning(string text, LogTag tag);

        protected abstract void InternalLogError(string text, LogTag tag);
    }
}