using System;
using System.IO;
using System.Threading.Tasks;
using NServiceBus;

static class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.Bridge.RightReceiver";
        var endpointConfiguration = new EndpointConfiguration("Samples.Bridge.RightReceiver");
        endpointConfiguration.UsePersistence<LearningPersistence>();

        endpointConfiguration.Conventions().DefiningCommandsAs(t => t.Name == "PlaceOrder");
        endpointConfiguration.Conventions().DefiningMessagesAs(t => t.Name == "OrderResponse");
        endpointConfiguration.Conventions().DefiningEventsAs(t => t.Name == "OrderReceived");

        #region alternative-learning-transport
        var learningTransportDefinition = new LearningTransport
        {
            // Set storage directory and add the character '2' to simulate a different transport.
            StorageDirectory = $"{LearningTransportInfrastructure.FindStoragePath()}2"
        };
        endpointConfiguration.UseTransport(learningTransportDefinition);
        #endregion

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