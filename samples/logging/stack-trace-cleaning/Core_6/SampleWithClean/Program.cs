using System;
using System.Threading.Tasks;
using NLog;
using NLog.Config;
using NLog.Targets;
using NServiceBus;

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

        NServiceBus.Logging.LogManager.Use<NLogFactory>();
        var endpointConfiguration = new EndpointConfiguration("Samples.SampleWithClean");

        #endregion

        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.EnableInstallers();
        #region disable-retries
        endpointConfiguration.Recoverability().Immediate(immediate => immediate.NumberOfRetries(0));
        endpointConfiguration.Recoverability().Delayed(delayed => delayed.NumberOfRetries(0));
        endpointConfiguration.SendFailedMessagesTo("error");
        #endregion
        #region customization-config
        endpointConfiguration.Recoverability().Failed(failed => failed.HeaderCustomization(StackTraceCleaner.CleanUp));
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