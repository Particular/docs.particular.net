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
        Console.Title = "Samples.SampleWithClean";

        #pragma warning disable CS0618 // Type or member is obsolete
        #region ConfigureNLog

        ConfigurationItemFactory.Default.LayoutRenderers
            .RegisterDefinition("customexception", typeof(CustomExceptionLayoutRenderer));
        var config = new LoggingConfiguration();

        var layout = "$|${logger}|${message}${onexception:${newline}${customexception:format=tostring}}";
        var consoleTarget = new ConsoleTarget
        {
            Layout = layout
        };
        config.AddTarget("console", consoleTarget);
        var consoleRule = new LoggingRule("*", LogLevel.Info, consoleTarget);
        config.LoggingRules.Add(consoleRule);
        var fileTarget = new FileTarget
        {
            FileName = "log.txt",
            Layout = layout
        };
        config.AddTarget("file", fileTarget);
        var fileRule = new LoggingRule("*", LogLevel.Info, fileTarget);
        config.LoggingRules.Add(fileRule);

        LogManager.Configuration = config;

        NServiceBus.Logging.LogManager.UseFactory(new ExtensionsLoggerFactory(new NLogLoggerFactory()));
        var endpointConfiguration = new EndpointConfiguration("Samples.SampleWithClean");

        #endregion
        #pragma warning restore CS0618 // Type or member is obsolete

        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseTransport(new LearningTransport());
        DisableRetries(endpointConfiguration);

        #region customization-config

        var recoverability = endpointConfiguration.Recoverability();
        recoverability.Failed(failed => failed.HeaderCustomization(StackTraceCleaner.CleanUp));

        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        await Run(endpointInstance)
            .ConfigureAwait(false);
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }

    static void DisableRetries(EndpointConfiguration endpointConfiguration)
    {
        #region disable-retries

        var recoverability = endpointConfiguration.Recoverability();
        recoverability.Immediate(
            customizations: immediate =>
            {
                immediate.NumberOfRetries(0);
            });
        recoverability.Delayed(
            customizations: delayed =>
            {
                delayed.NumberOfRetries(0);
            });

        #endregion
    }

    static async Task Run(IEndpointInstance endpointInstance)
    {
        Console.WriteLine("Press 'Enter' to send a Message");
        Console.WriteLine("Press any other key to exit");

        while (true)
        {
            var key = Console.ReadKey();
            if (key.Key != ConsoleKey.Enter)
            {
                return;
            }
            var message = new Message();
            await endpointInstance.SendLocal(message)
                .ConfigureAwait(false);
            Console.WriteLine();
            Console.WriteLine("Message sent");
        }
    }
}