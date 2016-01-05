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
        #region self-hosting

        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.SelfHosting");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.SendFailedMessagesTo("error");

        IEndpointInstance endpoint = await Endpoint.Start(busConfiguration);
        try
        {
            IBusSession busSession = endpoint.CreateBusSession();
            Console.WriteLine("\r\nBus created and configured; press any key to stop program\r\n");
            await busSession.SendLocal(new MyMessage());
            Console.ReadKey();
        }
        finally
        {
            await endpoint.Stop();
        }

        #endregion
    }
}