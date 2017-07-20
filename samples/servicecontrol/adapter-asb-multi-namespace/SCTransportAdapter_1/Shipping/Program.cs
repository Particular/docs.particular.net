using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Transport.AzureServiceBus;

class Program
{
    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Samples.ServiceControl.ASBAdapter.Shipping";
        var endpointConfiguration = new EndpointConfiguration(
            "Samples.ServiceControl.ASBAdapter.Shipping");

        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        var connectionString = Environment.GetEnvironmentVariable("AzureServiceBus.ConnectionString.2");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new Exception("Could not read the 'AzureServiceBus.ConnectionString.2' environment variable. Check the sample prerequisites.");
        }

        var salesConnectionString = Environment.GetEnvironmentVariable("AzureServiceBus.ConnectionString.1");
        if (string.IsNullOrWhiteSpace(salesConnectionString))
        {
            throw new Exception("Could not read the 'AzureServiceBus.ConnectionString.1' environment variable. Check the sample prerequisites.");
        }
        transport.ConnectionString(connectionString);
        transport.DefaultNamespaceAlias("shipping");
        endpointConfiguration.Recoverability().Failed(f => f.HeaderCustomization(h => h["NServiceBus.ASB.Namespace"] = "shipping"));
        transport.UseNamespaceAliasesInsteadOfConnectionStrings();
        transport.NamespaceRouting().AddNamespace("sales", salesConnectionString);
        transport.UseForwardingTopology();
        transport.BrokeredMessageBodyType(SupportedBrokeredMessageBodyTypes.Stream);

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