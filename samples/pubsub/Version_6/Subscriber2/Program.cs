using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Features;
using NServiceBus.Logging;

static class Program
{
    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        LogManager.Use<DefaultFactory>().Level(LogLevel.Info);
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.PubSub.Subscriber2");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.DisableFeature<AutoSubscribe>();
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.SendFailedMessagesTo("error");
        busConfiguration.EnableInstallers();

        IEndpointInstance endpoint = await Endpoint.Start(busConfiguration);
        try
        {
            IBusSession busSession = endpoint.CreateBusSession();
            await busSession.Subscribe<IMyEvent>();
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
            await busSession.Unsubscribe<IMyEvent>();
        }
        finally
        {
            await endpoint.Stop();
        }
    }
}