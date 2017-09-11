using NServiceBus;
using Microsoft.Extensions.Logging;
using NServiceBus.Logging;

class Usage
{
    Usage()
    {
        #region MsLoggingInCode

        using (var loggerFactory = new LoggerFactory())
        {
            loggerFactory.AddConsole();
            var logFactory = LogManager.Use<MicrosoftLogFactory>();
            logFactory.UseMsFactory(loggerFactory);

            
        }
        
        #endregion
    }

}
