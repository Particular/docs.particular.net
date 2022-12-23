using System;
using System.Threading.Tasks;
using NLog.Config;
using NLog.Extensions.Logging;
using NLog.Targets;
using NServiceBus;
using NServiceBus.Extensions.Logging;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.Logging.ExtensionsLogging";

        #region NLogConfiguration
        var config = new LoggingConfiguration();

        var consoleTarget = new ColoredConsoleTarget
        {
            Layout = "${level}|${logger}|${message}${onexception:${newline}${exception:format=tostring}}"
        };
        config.AddTarget("console", consoleTarget);
        config.LoggingRules.Add(new LoggingRule("*", NLog.LogLevel.Debug, consoleTarget));

        NLog.LogManager.Configuration = config;

        #endregion

        #region MicrosoftExtensionsLoggingNLog

        Microsoft.Extensions.Logging.ILoggerFactory extensionsLoggerFactory = new NLogLoggerFactory();
        
        NServiceBus.Logging.ILoggerFactory nservicebusLoggerFactory = new ExtensionsLoggerFactory(loggerFactory: extensionsLoggerFactory);

        NServiceBus.Logging.LogManager.UseFactory(loggerFactory:nservicebusLoggerFactory);

        #endregion

        var endpointConfiguration = new EndpointConfiguration("Samples.Logging.ExtensionsLogging");

        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseTransport<LearningTransport>();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        var myMessage = new MyMessage();
        await endpointInstance.SendLocal(myMessage)
            .ConfigureAwait(false);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}