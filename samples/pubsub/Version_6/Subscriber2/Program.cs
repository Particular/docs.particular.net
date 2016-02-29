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
        Console.Title = "Samples.PubSub.Subscriber2";
        LogManager.Use<DefaultFactory>().Level(LogLevel.Info);
        EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
        endpointConfiguration.EndpointName("Samples.PubSub.Subscriber2");
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.DisableFeature<AutoSubscribe>();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.EnableInstallers();

        IEndpointInstance endpoint = await Endpoint.Start(endpointConfiguration);
        try
        {
            await endpoint.Subscribe<IMyEvent>();
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
            await endpoint.Unsubscribe<IMyEvent>();
        }
        finally
        {
            await endpoint.Stop();
        }
    }
}