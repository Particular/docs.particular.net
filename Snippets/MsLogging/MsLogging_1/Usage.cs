using NServiceBus;
using Microsoft.Extensions.Logging;
using NServiceBus.Logging;
using Microsoft.Extensions.DependencyInjection;

class Usage
{
    Usage()
    {
        #region MsLoggingInCode

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddLogging(loggingBuilder =>
        {
            loggingBuilder.AddFilter(level => level >= Microsoft.Extensions.Logging.LogLevel.Information);
            loggingBuilder.AddConsole();
        });

        var serviceProvider = serviceCollection.BuildServiceProvider();

        using (var loggerFactory = new LoggerFactory())
        {
            var logFactory = LogManager.Use<MicrosoftLogFactory>();
            logFactory.UseMsFactory(loggerFactory);
            // endpoint startup and shutdown
        }

        #endregion
    }

}