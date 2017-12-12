using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Transport.AzureServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.ServiceControl.ASBAdapter.Shipping";
        var endpointConfiguration = new EndpointConfiguration("Samples.ServiceControl.ASBAdapter.Shipping");

        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        var connectionString = Environment.GetEnvironmentVariable("AzureServiceBus.ConnectionString.2");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new Exception("Could not read 'AzureServiceBus.ConnectionString.2' environment variable. Check sample prerequisites.");
        }

        var salesConnectionString = Environment.GetEnvironmentVariable("AzureServiceBus.ConnectionString.1");
        if (string.IsNullOrWhiteSpace(salesConnectionString))
        {
            throw new Exception("Could not read 'AzureServiceBus.ConnectionString.1' environment variable. Check sample prerequisites.");
        }
        transport.ConnectionString(connectionString);
        transport.DefaultNamespaceAlias("shipping");
        transport.UseNamespaceAliasesInsteadOfConnectionStrings();
        var routing = transport.NamespaceRouting();
        routing.AddNamespace("sales", salesConnectionString);
        transport.UseForwardingTopology();
        transport.BrokeredMessageBodyType(SupportedBrokeredMessageBodyTypes.Stream);
        transport.Composition().UseStrategy<HierarchyComposition>().PathGenerator(path => "scadapter/");

        endpointConfiguration.UsePersistence<InMemoryPersistence>();

        var chaos = new ChaosGenerator();
        endpointConfiguration.RegisterComponents(
            registration: components =>
            {
                components.ConfigureComponent(() => chaos, DependencyLifecycle.SingleInstance);
            });

        var recoverability = endpointConfiguration.Recoverability();
        recoverability.Failed(
            customizations: retryFailedSettings =>
            {
                retryFailedSettings.HeaderCustomization(
                    customization: headers =>
                    {
                        headers[AdapterSpecificHeaders.OriginalNamespace] = "shipping";
                    });
            });
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

        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.AuditProcessedMessagesTo("audit");
        endpointConfiguration.SendHeartbeatTo("Particular.ServiceControl");
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.UseSerialization<NewtonsoftSerializer>();

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