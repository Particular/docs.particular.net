using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.Pipeline.SigningAndEncryption.Endpoint";

        var endpointConfiguration = new EndpointConfiguration("Samples.Pipeline.SigningAndEncryption.Endpoint");
        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseTransport<LearningTransport>();

        #region configuration
        var pipeline = endpointConfiguration.Pipeline;
        pipeline.Register(new MessageSigningBehavior(), "Signs outgoing messages with a cryptographic signature");
        pipeline.Register(new SignatureVerificationBehavior(), "Validates the cryptographic signature on incoming messages");
        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        var key = default(ConsoleKeyInfo);

        Console.WriteLine("Press any key to send messages, 'q' to exit");
        while (key.KeyChar != 'q')
        {
            key = Console.ReadKey();

            var message = new MyMessage { Contents = Guid.NewGuid().ToString() };
            await endpointInstance.SendLocal(message)
                .ConfigureAwait(false);
        }

        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}