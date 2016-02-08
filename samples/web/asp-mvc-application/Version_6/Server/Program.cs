using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.Mvc.Server");
        busConfiguration.SendFailedMessagesTo("error");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.EnableInstallers();
        busConfiguration.SendFailedMessagesTo("error");

        IEndpointInstance endpoint = await Endpoint.Start(busConfiguration);
        try
        {
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
        finally
        {
            await endpoint.Stop();
        }
    }
}