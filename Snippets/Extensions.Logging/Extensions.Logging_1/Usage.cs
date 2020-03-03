using System.Threading.Tasks;
using NLog;	 
using NLog.Extensions.Logging;
using NServiceBus.Extensions.Logging;

class Usage
{
    void ExtensionsLogging()
    {
        ILoggerFactory extensionsLoggingFactory = ...;

        #region ExtensionsLogging

        LogManager.UseFactory(new ExtensionsLoggerFactory(extensionsLoggingFactory));

        #endregion
    }
}

