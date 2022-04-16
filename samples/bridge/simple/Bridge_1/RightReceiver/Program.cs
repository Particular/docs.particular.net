using System;
using System.IO;
using System.Threading.Tasks;
using NServiceBus;

static class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.PubSub.Subscriber";
        var endpointConfiguration = new EndpointConfiguration("Samples.PubSub.Subscriber");
        endpointConfiguration.UsePersistence<LearningPersistence>();
        var learningTransportDefinition = new LearningTransport
        {
            // Set storage directory and add the character '2' to mimic another transport.
            StorageDirectory = Path.Combine(LearningTransportInfrastructure.FindStoragePath(), "2")
        };
        endpointConfiguration.UseTransport(learningTransportDefinition);

        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.EnableInstallers();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}