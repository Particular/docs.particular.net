using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Features;
using AzureStorageQueueTransport = NServiceBus.AzureStorageQueueTransport;

class Program
{
    static async Task Main()
    {
        var endpointName = "Samples.Azure.StorageQueues.Endpoint2";
        Console.Title = endpointName;
        var endpointConfiguration = new EndpointConfiguration(endpointName);
        endpointConfiguration.UseSerialization<NewtonsoftSerializer>();
        endpointConfiguration.EnableInstallers();
        var transport = endpointConfiguration.UseTransport<AzureStorageQueueTransport>();
        transport.ConnectionString("UseDevelopmentStorage=true");
        transport.SanitizeQueueNamesWith(BackwardsCompatibleQueueNameSanitizer.WithMd5Shortener);
        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.DisableFeature<TimeoutManager>();
        endpointConfiguration.DisableFeature<MessageDrivenSubscriptions>();
        endpointConfiguration.SendFailedMessagesTo("error");

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}
