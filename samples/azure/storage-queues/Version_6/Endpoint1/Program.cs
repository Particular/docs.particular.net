using System;
using NServiceBus;

class Program
{

    static void Main()
    {
        Console.Title = "Samples.Azure.StorageQueues.Endpoint1";
        BusConfiguration busConfiguration = new BusConfiguration();
        #region config

        busConfiguration.EndpointName("Samples.Azure.StorageQueues.Endpoint1");
        busConfiguration.UseTransport<AzureStorageQueueTransport>();

        #endregion

        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.EnableInstallers();

        using (IBus bus = Bus.Create(busConfiguration).Start())
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
                bus.Send("Samples.Azure.StorageQueues.Endpoint2", message);
                Console.WriteLine("Message1 sent");
            }
        }
    }
}