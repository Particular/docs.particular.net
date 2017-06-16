﻿using System;
using System.Linq;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Transport.SQLServer;

class Program
{
    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Samples.ServiceControl.SqlServerTransportAdapter.Sales";
        const string letters = "ABCDEFGHIJKLMNOPQRSTUVXYZ";
        var random = new Random();
        var endpointConfiguration = new EndpointConfiguration("Samples.ServiceControl.SqlServerTransportAdapter.Sales");

        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
        var routing = transport.Routing();
        routing.RegisterPublisher(typeof(OrderShipped),
            "Samples.ServiceControl.SqlServerTransportAdapter.Shipping");
        transport.ConnectionString(@"Data Source=.\SQLEXPRESS;Initial Catalog=nservicebus;Integrated Security=True;Max Pool Size=100;Min Pool Size=10");


        #region SchemaV6

        //Use custom schema shipping for this endpoint
        transport.DefaultSchema("sales");

        //Configure schemas for the ServiceControl queues to point to the adapter
        transport.UseSchemaForQueue("audit", "adapter");
        transport.UseSchemaForQueue("error", "adapter");
        transport.UseSchemaForQueue("Particular.ServiceControl", "adapter");

        //Configure schema for the shipping endpoint so that the subscribe message is sent to the correct address
        transport.UseSchemaForQueue("Samples.ServiceControl.SqlServerTransportAdapter.Shipping", "shipping");

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
        recoverability.Delayed(delayed => delayed.NumberOfRetries(0));
        recoverability.DisableLegacyRetriesSatellite();

        var conventions = endpointConfiguration.Conventions();
        conventions.DefiningEventsAs(t => t == typeof(OrderAccepted) || t == typeof(OrderShipped));

        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.AuditProcessedMessagesTo("audit");
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
                var orderSubmitted = new OrderAccepted
                {
                    OrderId = orderId,
                    Value = random.Next(100)
                };
                await endpointInstance.Publish(orderSubmitted)
                    .ConfigureAwait(false);
            }
            if (lowerInvariant == 'f')
            {
                chaos.IsFailing = !chaos.IsFailing;
                Console.WriteLine("Failure simulation is now turned " + (chaos.IsFailing ? "on" : "off"));
            }
        }
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}