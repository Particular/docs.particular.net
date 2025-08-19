using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Features;
using AzureStorageQueueTransport = NServiceBus.AzureStorageQueueTransport;

class Program
{
    static async Task Main()
    {
        #region endpointName

        var endpointName = "Samples.Azure.StorageQueues.Endpoint1.With.A.Very.Long.Name.And.Invalid.Characters";
        var endpointConfiguration = new EndpointConfiguration(endpointName);

        #endregion

        Console.Title = endpointName;

        #region config

        var transport = endpointConfiguration.UseTransport<AzureStorageQueueTransport>();
        transport.ConnectionString("UseDevelopmentStorage=true");

        #endregion

        transport.DisablePublishing();
        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseSerialization<NewtonsoftJsonSerializer>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.DisableFeature<TimeoutManager>();

        #region sanitization

        transport.SanitizeQueueNamesWith(BackwardsCompatibleQueueNameSanitizer.WithMd5Shortener);

        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration);
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

            var message = new MyRequest
            {
                Property = "Hello from Endpoint1"
            };
            await endpointInstance.Send("Samples-Azure-StorageQueues-Endpoint2", message);
            Console.WriteLine("MyRequest sent");
        }
        await endpointInstance.Stop();
    }
}
