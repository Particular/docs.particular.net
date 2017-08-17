using System;
using NServiceBus;

class Program
{

    static void Main()
    {
        Console.Title = "Samples.Azure.StorageQueues.Endpoint1";
        var busConfiguration = new BusConfiguration();
        #region config

        busConfiguration.EndpointName("Samples.Azure.StorageQueues.Endpoint1");
        busConfiguration.UseTransport<AzureStorageQueueTransport>()
            .ConnectionString("UseDevelopmentStorage=true");

        #endregion

        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.EnableInstallers();

        using (var bus = Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("Press 'enter' to send a message");
            Console.WriteLine("Press any other key to exit");

            while (true)
            {
                var key = Console.ReadKey();
                Console.WriteLine();

                if (key.Key != ConsoleKey.Enter)
                {
                    return;
                }

                var orderId = Guid.NewGuid();
                var message = new Message1
                {
                    Property = "Hello from Endpoint1"
                };
                bus.Send("Samples.Azure.StorageQueues.Endpoint2", message);
                Console.WriteLine("Message1 sent");
            }
        }
    }
}