using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.Pipeline.SigningAndEncryption.UnsignedSender";

        var endpointConfiguration = new EndpointConfiguration("Samples.Pipeline.SigningAndEncryption.UnsignedSender");
        endpointConfiguration.UsePersistence<LearningPersistence>();

        var transport = endpointConfiguration.UseTransport<LearningTransport>();
        var routing = transport.Routing();
        routing.RouteToEndpoint(typeof(MyMessage), "Samples.Pipeline.SigningAndEncryption.ReceivingEndpoint");

        // This endpoint does not sign messages.
        // Do not call endpointConfiguration.RegisterSigningBehaviors() here.

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        var key = default(ConsoleKeyInfo);

        Console.WriteLine("Press any key to send messages, 'q' to exit");
        while (key.KeyChar != 'q')
        {
            key = Console.ReadKey();

            var options = new SendOptions();
            options.SetHeader("X-Message-Header", "Fake signature, obviously not correct!");
            var message = new MyMessage { Contents = Guid.NewGuid().ToString() };
            await endpointInstance.Send(message, options)
                .ConfigureAwait(false);
        }

        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}