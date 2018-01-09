using System;
using System.Linq;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.ServiceControl.RabbitMQAdapter.Sales";
        const string letters = "ABCDEFGHIJKLMNOPQRSTUVXYZ";
        var random = new Random();

        #region SalesConfiguration

        var endpointConfiguration = new EndpointConfiguration("Samples.ServiceControl.RabbitMQAdapter.Sales");

        var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
        transport.ConnectionString("host=localhost");
        transport.DelayedDelivery().DisableTimeoutManager();

        transport.UseDirectRoutingTopology();

        endpointConfiguration.SendFailedMessagesTo("adapter_error");
        endpointConfiguration.AuditProcessedMessagesTo("adapter_audit");

        #endregion

        var recoverability = endpointConfiguration.Recoverability();

        endpointConfiguration.UsePersistence<InMemoryPersistence>();

        var chaos = new ChaosGenerator();
        endpointConfiguration.RegisterComponents(
            registration: components =>
            {
                components.ConfigureComponent(() => chaos, DependencyLifecycle.SingleInstance);
            });

        recoverability.Immediate(
            customizations: immediate =>
            {
                immediate.NumberOfRetries(0);
            });
        recoverability.Delayed(delayed => delayed.NumberOfRetries(0));

        var routingConfig = transport.Routing();
        routingConfig.RouteToEndpoint(typeof(ShipOrder).Assembly, "Samples.ServiceControl.RabbitMQAdapter.Shipping");

        endpointConfiguration.EnableInstallers();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Press enter to exit");
        Console.WriteLine("Press 'o' to generate order");
        Console.WriteLine("Press 'f' to toggle simulating of message processing failure");
        while (true)
        {
            var key = Console.ReadKey();
            Console.WriteLine();
            if (key.Key == ConsoleKey.Enter)
            {
                break;
            }
            var lowerInvariant = char.ToLowerInvariant(key.KeyChar);
            if (lowerInvariant == 'o')
            {
                var orderId = new string(Enumerable.Range(0, 4).Select(x => letters[random.Next(letters.Length)]).ToArray());
                var shipOrder = new ShipOrder
                {
                    OrderId = orderId,
                    Value = random.Next(100)
                };
                await endpointInstance.Send(shipOrder)
                    .ConfigureAwait(false);
            }
            if (lowerInvariant == 'f')
            {
                chaos.IsFailing = !chaos.IsFailing;
                Console.WriteLine($"Failure simulation is now turned {(chaos.IsFailing ? "on" : "off")}");
                ConsoleHelper.ToggleTitle();
            }
        }
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}
