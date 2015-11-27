using System;
using System.Threading;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

class Program
{
    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        LogManager.Use<DefaultFactory>().Level(LogLevel.Error);
        #region Program
        Logger.WriteLine("Starting configuration");
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.StartupShutdown");
        busConfiguration.EnableInstallers();
        busConfiguration.EnableFeature<MyFeature>();
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.SendFailedMessagesTo("error");

        Logger.WriteLine("Calling Bus.Create");
        IEndpointInstance endpoint = await Endpoint.Start(busConfiguration);
        try
        {
            Logger.WriteLine("Calling IEndpointInstance.CreateBusContext");
            IBusContext busContext = endpoint.CreateBusContext();

            //simulate some bus activity
            Thread.Sleep(500);

            Logger.WriteLine("Bus is processing messages");
        }
        finally
        {
            Logger.WriteLine("Calling IEndpointInstance.Stop");
            endpoint.Stop().GetAwaiter().GetResult();
        }
        Logger.WriteLine("Finished");
        #endregion
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
    }
}