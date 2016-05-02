using Common.Logging;
using Common.Logging.Simple;
using NServiceBus;
using NServiceBus.Logging.Loggers;

class Usage
{
    Usage()
    {
        #region CommonLoggingInCode

        LogManager.Adapter = new ConsoleOutLoggerFactoryAdapter();

        SetLoggingLibrary.Custom(new ConsoleLoggerFactory());

        #endregion
    }
}
