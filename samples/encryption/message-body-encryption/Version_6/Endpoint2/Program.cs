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
        Console.Title = "Samples.MessageBodyEncryption.Endpoint2";
        EndpointConfiguration endpointConfiguration = new EndpointConfiguration("Samples.MessageBodyEncryption.Endpoint2");
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.RegisterMessageEncryptor();
        endpointConfiguration.SendFailedMessagesTo("error");
        IEndpointInstance endpoint = await Endpoint.Start(endpointConfiguration);
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