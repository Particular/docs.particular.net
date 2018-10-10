using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Features;

static class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.EditMessage.Publisher";
        var endpointConfiguration = new EndpointConfiguration("Samples.EditMessage.Publisher");
        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.DisableFeature<TimeoutManager>();
        endpointConfiguration.UseTransport<MsmqTransport>();

        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.AuditProcessedMessagesTo("audit");
        endpointConfiguration.EnableInstallers();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        await Start(endpointInstance)
            .ConfigureAwait(false);
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }

    static async Task Start(IEndpointInstance endpointInstance)
    {
        Console.WriteLine("This application sends PlaceOrder command with wrong UserId, to ilustrate how such messages can be modified using ServiceControl and ServicePulse.");
        Console.WriteLine("Press '1' to send the PlaceOrder command");
        Console.WriteLine("Press any other key to exit");

        #region PublishLoop

        while (true)
        {
            var key = Console.ReadKey();
            Console.WriteLine();

            var orderId = Guid.NewGuid();
            var nonExistantUserId = 11; 
            if (key.Key == ConsoleKey.D1)
            {
                var placeOrder = new PlaceOrder
                {
                    OrderId = orderId,
                    UserId = nonExistantUserId
                };
                var options = new SendOptions();
                options.SetDestination("Samples.EditMessage.Subscriber");
                await endpointInstance.Send(placeOrder, options)
                    .ConfigureAwait(false);
                Console.WriteLine($"Send PlaceOrder command with Id {orderId}.");
            }
            else
            {
                return;
            }
        }

        #endregion
    }
}