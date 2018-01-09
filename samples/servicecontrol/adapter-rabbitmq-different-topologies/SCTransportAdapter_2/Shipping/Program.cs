using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.ServiceControl.RabbitMQAdapter.Shipping";

        #region ShippingConfiguration

        var endpointConfiguration = new EndpointConfiguration("Samples.ServiceControl.RabbitMQAdapter.Shipping");

        var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
        transport.ConnectionString("host=localhost");
        transport.DelayedDelivery().DisableTimeoutManager();

        transport.UseDirectRoutingTopology();

        endpointConfiguration.SendFailedMessagesTo("adapter_error");
        endpointConfiguration.AuditProcessedMessagesTo("adapter_audit");
        endpointConfiguration.SendHeartbeatTo("adapter_Particular.ServiceControl");

        #endregion

        endpointConfiguration.UsePersistence<InMemoryPersistence>();

        var chaos = new ChaosGenerator();
        endpointConfiguration.RegisterComponents(
            registration: components =>
            {
                components.ConfigureComponent(() => chaos, DependencyLifecycle.SingleInstance);
            });

        var recoverability = endpointConfiguration.Recoverability();
        recoverability.Immediate(
            customizations: immediate =>
            {
                immediate.NumberOfRetries(0);
            });
        recoverability.Delayed(
            customizations: delayed =>
            {
                delayed.NumberOfRetries(0);
            });
        endpointConfiguration.EnableInstallers();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Press enter to exit");
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