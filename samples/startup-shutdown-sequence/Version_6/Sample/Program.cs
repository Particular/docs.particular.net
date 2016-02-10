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
        EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
        endpointConfiguration.EndpointName("Samples.StartupShutdown");
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.EnableFeature<MyFeature>();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.SendFailedMessagesTo("error");

        Logger.WriteLine("Calling Bus.Create");
        IEndpointInstance endpoint = await Endpoint.Start(endpointConfiguration);
        try
        {
            //simulate some bus activity
            Thread.Sleep(500);

            Logger.WriteLine("Bus is processing messages");
        }
        finally
        {
            Logger.WriteLine("Calling IEndpointInstance.Stop");
            await endpoint.Stop();
        }
        Logger.WriteLine("Finished");
        #endregion
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
    }
}