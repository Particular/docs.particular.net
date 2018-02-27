using System;
using NServiceBus;
using NServiceBus.Features;
using NServiceBus.Transports.SQLServer;

class Program
{
    static void Main()
    {
        var connectionString = @"Data Source=.\SqlExpress;Initial Catalog=nservicebus;Integrated Security=True;Max Pool Size=100;Min Pool Size=10";

        Console.Title = "Samples.ServiceControl.SqlServerTransportAdapter.Shipping";
        var endpointConfiguration = new BusConfiguration();
        endpointConfiguration.EndpointName("Samples.ServiceControl.SqlServerTransportAdapter.Shipping");

        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
        transport.ConnectionString(connectionString);

        SqlHelper.EnsureDatabaseExists(connectionString);
        SqlHelper.CreateSchema(connectionString, "shipping");
        SqlHelper.CreateSchema(connectionString, "adapter");

        #region SchemaV5

        //Use custom schema shipping for this endpoint
        transport.DefaultSchema("shipping");

        transport.UseSpecificConnectionInformation(
            //Configure schema for sales endpoint so that the subscribe message is sent to the correct address
            EndpointConnectionInfo.For("Samples.ServiceControl.SqlServerTransportAdapter.Sales").UseSchema("sales"),
            //Configure schemas for ServiceControl queues to point to the adapter
            EndpointConnectionInfo.For("audit").UseSchema("adapter"),
            EndpointConnectionInfo.For("error").UseSchema("adapter"),
            EndpointConnectionInfo.For("Particular.ServiceControl").UseSchema("adapter")
        );

        #endregion

        endpointConfiguration.UsePersistence<InMemoryPersistence>();

        var chaos = new ChaosGenerator();
        endpointConfiguration.RegisterComponents(
            registration: components =>
            {
                components.ConfigureComponent(() => chaos, DependencyLifecycle.SingleInstance);
            });

        endpointConfiguration.DisableFeature<SecondLevelRetries>();

        var conventions = endpointConfiguration.Conventions();
        conventions.DefiningEventsAs(t => t == typeof(OrderAccepted) || t == typeof(OrderShipped));

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
}