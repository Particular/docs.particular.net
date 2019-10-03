using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.Pipeline.SigningAndEncryption.ReceivingEndpoint";

        var endpointConfiguration = new EndpointConfiguration("Samples.Pipeline.SigningAndEncryption.ReceivingEndpoint");
        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseTransport<LearningTransport>();

        endpointConfiguration.RegisterSigningBehaviors();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        Console.WriteLine("Waiting to receive messages. Press Enter to exit.");
        Console.ReadLine();

        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}