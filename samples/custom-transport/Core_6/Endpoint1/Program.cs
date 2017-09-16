using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Features;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.CustomTransport.Endpoint1";
        var endpointConfiguration = new EndpointConfiguration("Samples.CustomTransport.Endpoint1");

        #region UseDefinition

        endpointConfiguration.UseTransport<FileTransport>();

        #endregion

        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.DisableFeature<TimeoutManager>();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        #region StartMessageInteraction

        var messageA = new MessageA();
        await endpointInstance.Send("Samples.CustomTransport.Endpoint2", messageA)
            .ConfigureAwait(false);

        #endregion

        Console.WriteLine("MessageA sent. Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}