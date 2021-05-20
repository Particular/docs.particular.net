using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        var endpointName = "Samples.ServiceControl.ASQAdapter.Shipping";

        Console.Title = endpointName;

        var endpointConfiguration = new EndpointConfiguration("Samples-ServiceControl-ASQAdapter-Shipping");

        var transport = endpointConfiguration.UseTransport<AzureStorageQueueTransport>();
        var connectionString = Environment.GetEnvironmentVariable("AzureStorageQueue.ConnectionString.Endpoints");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new Exception("Could not read 'AzureStorageQueue.ConnectionString.Endpoints' environment variable. Check sample prerequisites.");
        }

        transport.ConnectionString(connectionString);
        transport.DefaultAccountAlias("storage_account");

        // Required to address https://github.com/Particular/NServiceBus.AzureStorageQueues/issues/308
        transport.AccountRouting().AddAccount("storage_account", connectionString);

        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.UseSerialization<NewtonsoftSerializer>();

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
                        headers[AdapterSpecificHeaders.OriginalStorageAccountAlias] = "storage_account";
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
        endpointConfiguration.SendHeartbeatTo("Particular-ServiceControl");
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
                ConsoleHelper.ToggleTitle(endpointName);
            }
        }
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}