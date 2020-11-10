using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Features;
using AzureStorageQueueTransport = NServiceBus.AzureStorageQueueTransport;

class Program
{
    static async Task Main()
    {
        var endpointName = "Samples-Azure-StorageQueues-Endpoint1";
        var endpointConfiguration = new EndpointConfiguration(endpointName);

        Console.Title = endpointName;

        #region config

        var transport = endpointConfiguration.UseTransport<AzureStorageQueueTransport>();
        transport.ConnectionString("UseDevelopmentStorage=true");

        #endregion

        transport.DisablePublishing();
        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseSerialization<NewtonsoftSerializer>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.DisableFeature<TimeoutManager>();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Press 'enter' to send a message");
        Console.WriteLine("Press any other key to exit");

        while (true)
        {
            var key = Console.ReadKey();
            Console.WriteLine();

            if (key.Key != ConsoleKey.Enter)
            {
                break;
            }

            var message = new Message1
            {
                Property = "Hello from Endpoint1"
            };
            await endpointInstance.Send("Samples-Azure-StorageQueues-Endpoint2", message)
                .ConfigureAwait(false);
            Console.WriteLine("Message1 sent");
        }
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}
