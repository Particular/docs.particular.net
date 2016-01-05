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
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {

        #region ConfigureNLog

        LoggingConfiguration config = new LoggingConfiguration();

        ColoredConsoleTarget consoleTarget = new ColoredConsoleTarget
                                             {
                                                 Layout = "${level}|${logger}|${message}${onexception:${newline}${exception:format=tostring}}"
                                             };
        config.AddTarget("console", consoleTarget);
        config.LoggingRules.Add(new LoggingRule("*", LogLevel.Info, consoleTarget));

        LogManager.Configuration = config;
        #endregion
        #region UseConfig

        NServiceBus.Logging.LogManager.Use<NLogFactory>();

        // Then continue with your BusConfiguration
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.Logging.NLogCustom");

        #endregion

        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.SendFailedMessagesTo("error");

        IEndpointInstance endpoint = await Endpoint.Start(busConfiguration);
        try
        {
            IBusSession busSession = endpoint.CreateBusSession();
            await busSession.SendLocal(new MyMessage());
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
        finally
        {
            await endpoint.Stop();
        }
    }
}
