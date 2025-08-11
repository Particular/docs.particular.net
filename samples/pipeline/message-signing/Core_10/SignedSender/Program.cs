using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "SignedSender";

        var endpointConfiguration = new EndpointConfiguration("Samples.Pipeline.SigningAndEncryption.SignedSender");
        endpointConfiguration.UsePersistence<LearningPersistence>();

        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        var routing = endpointConfiguration.UseTransport(new LearningTransport());
        routing.RouteToEndpoint(typeof(MyMessage), "Samples.Pipeline.SigningAndEncryption.ReceivingEndpoint");

        #region EnableSigning

        endpointConfiguration.RegisterSigningBehaviors();

        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration);

        var key = default(ConsoleKeyInfo);

        Console.WriteLine("Press any key to send messages, 'q' to exit");
        while (key.KeyChar != 'q')
        {
            key = Console.ReadKey();

            var message = new MyMessage { Contents = Guid.NewGuid().ToString() };
            await endpointInstance.Send(message);
        }

        await endpointInstance.Stop();
    }
}
