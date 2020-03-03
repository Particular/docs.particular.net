using Microsoft.Extensions.Logging;
using NServiceBus.Extensions.Logging;

class Usage
{
    void ExtensionsLogging()
    {
        ILoggerFactory extensionsLoggingFactory = null;

        #region ExtensionsLogging

        NServiceBus.Logging.LogManager.UseFactory(new ExtensionsLoggerFactory(extensionsLoggingFactory));

        #endregion
    }
}

