using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Features;

class Program
{
    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Samples.CustomTransport.Endpoint1";
        var endpointConfiguration = new EndpointConfiguration("Samples.CustomTransport.Endpoint1");

        #region UseDefinition

        endpointConfiguration.UseTransport<FileTransport>();

        #endregion

        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.DisableFeature<TimeoutManager>();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        try
        {
            #region StartMessageInteraction

            var messageA = new MessageA();
            await endpointInstance.Send("Samples.CustomTransport.Endpoint2", messageA)
                .ConfigureAwait(false);

            #endregion

            Console.WriteLine("MessageA sent. Press any key to exit");
            Console.ReadKey();
        }
        finally
        {
            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }
    }
}