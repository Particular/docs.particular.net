using Common.Logging;
using Common.Logging.Simple;
using NServiceBus;

public class CommonLoggingConfig
{
    public void InCode()
    {
        #region CommonLoggingInCode

        LogManager.Adapter = new ConsoleOutLoggerFactoryAdapter();

        NServiceBus.Logging.LogManager.Use<CommonLoggingFactory>();

        #endregion
    }
}