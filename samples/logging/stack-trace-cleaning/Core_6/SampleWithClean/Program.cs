using System;
using System.Threading.Tasks;
using NLog;
using NLog.Config;
using NLog.Targets;
using NServiceBus;
using NServiceBus.Features;

class Program
{

    static void Main()
    {
        Start().GetAwaiter().GetResult();
    }

    static async Task Start()
    {
        Console.Title = "Samples.SampleWithClean";

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
        config.LoggingRules.Add(new LoggingRule("*", LogLevel.Info, consoleTarget));
        var fileTarget = new FileTarget
        {
            FileName = "log.txt",
            Layout = layout
        };
        config.AddTarget("file", fileTarget);
        config.LoggingRules.Add(new LoggingRule("*", LogLevel.Info, fileTarget));

        LogManager.Configuration = config;

        NServiceBus.Logging.LogManager.Use<NLogFactory>();
        var endpointConfiguration = new EndpointConfiguration("Samples.SampleWithClean");

        #endregion

        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.EnableInstallers();
        #region disable-retries
        endpointConfiguration.DisableFeature<FirstLevelRetries>();
        endpointConfiguration.DisableFeature<SecondLevelRetries>();
        endpointConfiguration.SendFailedMessagesTo("error");
        #endregion
        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        try
        {
            await Run(endpointInstance)
                .ConfigureAwait(false);
        }
        finally
        {
            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }
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