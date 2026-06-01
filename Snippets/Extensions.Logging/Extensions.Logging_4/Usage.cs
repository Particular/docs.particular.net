using Microsoft.Extensions.Logging;
using NServiceBus.Extensions.Logging;

class Usage
{
    void ExtensionsLogging()
    {
        ILoggerFactory extensionsLoggingFactory = null;

#pragma warning disable CS0618 // Type or member is obsolete
        #region ExtensionsLogging

        NServiceBus.Logging.LogManager.UseFactory(new ExtensionsLoggerFactory(extensionsLoggingFactory));

        #endregion
#pragma warning restore CS0618 // Type or member is obsolete
    }
}

