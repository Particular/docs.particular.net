using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Transport.SQLServer;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.ServiceControl.SqlServerTransportAdapter.Shipping";
        var endpointConfiguration = new EndpointConfiguration(
            "Samples.ServiceControl.SqlServerTransportAdapter.Shipping");

        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
        var routing = transport.Routing();
        routing.RegisterPublisher(typeof(OrderAccepted),
            "Samples.ServiceControl.SqlServerTransportAdapter.Sales");
#pragma warning disable 618
        transport.EnableLegacyMultiInstanceMode(Connections.GetConnection);
#pragma warning restore 618

        SqlHelper.EnsureDatabaseExists(Connections.Shipping);

        endpointConfiguration.UsePersistence<InMemoryPersistence>();

        var chaos = new ChaosGenerator();
        endpointConfiguration.RegisterComponents(c =>
        {
            c.ConfigureComponent(() => chaos, DependencyLifecycle.SingleInstance);
        });

        var recoverability = endpointConfiguration.Recoverability();
        recoverability.Immediate(immediate => immediate.NumberOfRetries(0));
        recoverability.Delayed(delayed => delayed.NumberOfRetries(0));
        recoverability.DisableLegacyRetriesSatellite();

        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.AuditProcessedMessagesTo("audit");
        endpointConfiguration.HeartbeatPlugin("Particular.ServiceControl");
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
            }
        }
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}