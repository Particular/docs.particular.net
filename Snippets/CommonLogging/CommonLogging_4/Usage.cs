using Common.Logging;
using Common.Logging.Simple;
using NServiceBus;

class Usage
{
    Usage()
    {
        #region CommonLoggingInCode

        LogManager.Adapter = new ConsoleOutLoggerFactoryAdapter();
        NServiceBus.Logging.LogManager.Use<CommonLoggingFactory>();

        #endregion
    }
}
