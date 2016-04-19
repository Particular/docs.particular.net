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
        Console.Title = "Samples.SampleWithoutClean";

        LoggingConfiguration config = new LoggingConfiguration();

        string layout = "${logger}|${message}${onexception:${newline}${exception:format=tostring}}";
        ConsoleTarget consoleTarget = new ConsoleTarget
        {
            Layout = layout
        };
        config.AddTarget("console", consoleTarget);
        config.LoggingRules.Add(new LoggingRule("*", LogLevel.Info, consoleTarget));
        FileTarget fileTarget = new FileTarget
        {
            FileName = "log.txt",
            Layout = layout
        };
        config.AddTarget("file", fileTarget);
        config.LoggingRules.Add(new LoggingRule("*", LogLevel.Info, fileTarget));

        LogManager.Configuration = config;

        NServiceBus.Logging.LogManager.Use<NLogFactory>();
        EndpointConfiguration endpointConfiguration = new EndpointConfiguration("Samples.SampleWithoutClean");
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.DisableFeature<FirstLevelRetries>();
        endpointConfiguration.DisableFeature<SecondLevelRetries>();
        endpointConfiguration.SendFailedMessagesTo("error");

        IEndpointInstance endpoint = await Endpoint.Start(endpointConfiguration);
        try
        {
            await Run(endpoint);
        }
        finally
        {
            await endpoint.Stop();
        }
    }

    static async Task Run(IEndpointInstance endpointInstance)
    {
        Console.WriteLine("Press 'Enter' to send a Message");
        Console.WriteLine("Press any other key to exit");

        while (true)
        {
            ConsoleKeyInfo key = Console.ReadKey();
            if (key.Key != ConsoleKey.Enter)
            {
                return;
            }
            Message message = new Message();
            await endpointInstance.SendLocal(message);
            Console.WriteLine();
            Console.WriteLine("Message sent");
        }
    }
}