using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.Pipeline.SigningAndEncryption.SignedSender";

        var endpointConfiguration = new EndpointConfiguration("Samples.Pipeline.SigningAndEncryption.SignedSender");
        endpointConfiguration.UsePersistence<LearningPersistence>();

        var transport = endpointConfiguration.UseTransport<LearningTransport>();
        var routing = transport.Routing();
        routing.RouteToEndpoint(typeof(MyMessage), "Samples.Pipeline.SigningAndEncryption.ReceivingEndpoint");

        #region EnableSigning

        endpointConfiguration.RegisterSigningBehaviors();

        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        var key = default(ConsoleKeyInfo);

        Console.WriteLine("Press any key to send messages, 'q' to exit");
        while (key.KeyChar != 'q')
        {
            key = Console.ReadKey();

            var message = new MyMessage { Contents = Guid.NewGuid().ToString() };
            await endpointInstance.Send(message)
                .ConfigureAwait(false);
        }

        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}