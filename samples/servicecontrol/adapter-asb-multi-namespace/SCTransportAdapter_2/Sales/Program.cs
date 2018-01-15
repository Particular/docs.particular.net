using System;
using System.Linq;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Transport.AzureServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.ServiceControl.ASBAdapter.Sales";
        const string letters = "ABCDEFGHIJKLMNOPQRSTUVXYZ";
        var random = new Random();
        var endpointConfiguration = new EndpointConfiguration("Samples.ServiceControl.ASBAdapter.Sales");

        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        var connectionString = Environment.GetEnvironmentVariable("AzureServiceBus.ConnectionString.1");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new Exception("Could not read 'AzureServiceBus.ConnectionString.1' environment variable. Check sample prerequisites.");
        }
        var shippingConnectionString = Environment.GetEnvironmentVariable("AzureServiceBus.ConnectionString.2");
        if (string.IsNullOrWhiteSpace(shippingConnectionString))
        {
            throw new Exception("Could not read 'AzureServiceBus.ConnectionString.2' environment variable. Check sample prerequisites.");
        }
        transport.ConnectionString(connectionString);

        #region FeaturesUnsuportedBySC

        transport.UseNamespaceAliasesInsteadOfConnectionStrings();
        transport.DefaultNamespaceAlias("sales");
        var routing = transport.NamespaceRouting();
        routing.AddNamespace("shipping", shippingConnectionString);
        transport.UseForwardingTopology();
        transport.BrokeredMessageBodyType(SupportedBrokeredMessageBodyTypes.Stream);
        var composition = transport.Composition();
        composition.UseStrategy<HierarchyComposition>()
            .PathGenerator(path => "scadapter/");

        #endregion

        var recoverability = endpointConfiguration.Recoverability();

        #region NamespaceAlias

        recoverability.Failed(
            customizations: settings =>
            {
                settings.HeaderCustomization(
                    customization: headers =>
                    {
                        headers[AdapterSpecificHeaders.OriginalNamespace] = "sales";
                    });
            });

        #endregion

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

        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.AuditProcessedMessagesTo("audit");
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.UseSerialization<NewtonsoftSerializer>();

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
                var sendOptions = new SendOptions();
                sendOptions.SetDestination("Samples.ServiceControl.ASBAdapter.Shipping@shipping");
                await endpointInstance.Send(shipOrder, sendOptions)
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