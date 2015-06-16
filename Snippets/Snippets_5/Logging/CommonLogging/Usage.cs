namespace Snippets5.Logging.CommonLogging
{
    using Common.Logging;
    using Common.Logging.Simple;
    using NServiceBus;

    public class Usage
    {
        public Usage()
        {
            #region CommonLoggingInCode

            LogManager.Adapter = new ConsoleOutLoggerFactoryAdapter();

            NServiceBus.Logging.LogManager.Use<CommonLoggingFactory>();

            #endregion
        }
    }
}