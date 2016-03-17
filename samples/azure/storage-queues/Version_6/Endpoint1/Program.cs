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
        Console.Title = "Samples.Azure.StorageQueues.Endpoint1";
        EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
        #region config

        endpointConfiguration.EndpointName("Samples.Azure.StorageQueues.Endpoint1");
        endpointConfiguration.UseTransport<AzureStorageQueueTransport>();

        #endregion

        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.SendFailedMessagesTo("error");

        IEndpointInstance endpoint = await Endpoint.Start(endpointConfiguration);
        try
        {
            Console.WriteLine("Press 'enter' to send a message");
            Console.WriteLine("Press any other key to exit");

            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey();
                Console.WriteLine();

                if (key.Key != ConsoleKey.Enter)
                {
                    return;
                }

                Guid orderId = Guid.NewGuid();
                Message1 message = new Message1
                {
                    Property = "Hello from Endpoint1"
                };
                await endpoint.Send("Samples.Azure.StorageQueues.Endpoint2", message);
                Console.WriteLine("Message1 sent");
            }
        }
        finally
        {
            await endpoint.Stop();
        }
    }
}