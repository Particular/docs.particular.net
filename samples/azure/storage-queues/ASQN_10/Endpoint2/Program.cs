using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        var endpointName = "Samples-Azure-StorageQueues-Endpoint2";
        Console.Title = endpointName;
        var endpointConfiguration = new EndpointConfiguration(endpointName);
        endpointConfiguration.UseSerialization<NewtonsoftSerializer>();
        endpointConfiguration.EnableInstallers();
        var transport = new AzureStorageQueueTransport("UseDevelopmentStorage=true", useNativeDelayedDeliveries: false)
        {
            QueueNameSanitizer = BackwardsCompatibleQueueNameSanitizer.WithMd5Shortener
        };
        var routingSettings = endpointConfiguration.UseTransport(transport);
        routingSettings.DisablePublishing();
        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.SendFailedMessagesTo("error");

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}
