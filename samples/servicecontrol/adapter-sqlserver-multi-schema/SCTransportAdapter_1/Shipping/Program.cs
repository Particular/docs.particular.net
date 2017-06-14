using System;
using System.Linq;
using NServiceBus;
using NServiceBus.Features;
using NServiceBus.Transports.SQLServer;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.ServiceControl.SqlServerTransportAdapter.Shipping";
        var endpointConfiguration = new BusConfiguration();
        endpointConfiguration.EndpointName("Samples.ServiceControl.SqlServerTransportAdapter.Shipping");

        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();

        //Use schema shipping for this endpoint
        transport.DefaultSchema("shipping");
        transport.ConnectionString(@"Data Source=.\SQLEXPRESS;Initial Catalog=nservicebus;Integrated Security=True;Max Pool Size=100;Min Pool Size=10");

        //Use this callback to get the connection information for other queues
        transport.UseSpecificConnectionInformation(
            EndpointConnectionInfo.For("Samples.ServiceControl.SqlServerTransportAdapter.Sales").UseSchema("sales"),
            EndpointConnectionInfo.For("audit").UseSchema("adapter"),
            EndpointConnectionInfo.For("error").UseSchema("adapter"),
            EndpointConnectionInfo.For("Particular.ServiceControl").UseSchema("adapter")
            );

        endpointConfiguration.UsePersistence<InMemoryPersistence>();

        var chaos = new ChaosGenerator();
        endpointConfiguration.RegisterComponents(c =>
        {
            c.ConfigureComponent(() => chaos, DependencyLifecycle.SingleInstance);
        });

        endpointConfiguration.DisableFeature<SecondLevelRetries>();

        endpointConfiguration.Conventions().DefiningEventsAs(t => t == typeof(OrderAccepted) || t == typeof(OrderShipped));

        endpointConfiguration.EnableInstallers();

        using (var endpointInstance = Bus.Create(endpointConfiguration).Start())
        {
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
        }
    }

    #region GetConnectionString2

    static readonly string[] AdapterQueues = { "audit", "error", "poison", "Particular.ServiceControl" };

    static ConnectionInfo GetConnectionInfo(string destination)
    {
        if (destination.StartsWith("Samples.ServiceControl.SqlServerTransportAdapter.Sales", StringComparison.OrdinalIgnoreCase))
        {
            return ConnectionInfo.Create().UseSchema("sales");
        }
        if (destination.StartsWith("Samples.ServiceControl.SqlServerTransportAdapter.Shipping", StringComparison.OrdinalIgnoreCase))
        {
            return ConnectionInfo.Create().UseSchema("shipping");
        }
        if (AdapterQueues.Any(q => string.Equals(q, destination, StringComparison.OrdinalIgnoreCase)))
        {
            return ConnectionInfo.Create().UseSchema("adapter");
        }
        throw new Exception($"Unknown destination: {destination}");
    }

    #endregion
}