using System;
using System.Threading.Tasks;
using NLog;
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

        var config = new LoggingConfiguration();

        var consoleTarget = new ColoredConsoleTarget
        {
            Layout = "${level}|${logger}|${message}${onexception:${newline}${exception:format=tostring}}"
        };
        config.AddTarget("console", consoleTarget);
        config.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, consoleTarget));

        NLog.LogManager.Configuration = config;

        NServiceBus.Logging.LogManager.UseFactory(new ExtensionsLoggerFactory(new NLogLoggerFactory()));

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