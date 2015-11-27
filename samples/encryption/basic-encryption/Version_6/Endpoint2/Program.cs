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
        busConfiguration.EndpointName("Samples.Encryption.Endpoint2");
        busConfiguration.RijndaelEncryptionService();
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.SendFailedMessagesTo("error");
        IEndpointInstance endpoint = await Endpoint.Start(busConfiguration);
        try
        {
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
        finally
        {
            endpoint.Stop().GetAwaiter().GetResult();
        }
    }
}