using System;
using System.Linq;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        var endpointName = "Samples.ServiceControl.ASQAdapter.Sales";

        Console.Title = endpointName;

        const string letters = "ABCDEFGHIJKLMNOPQRSTUVXYZ";
        var random = new Random();
        var endpointConfiguration = new EndpointConfiguration("Samples-ServiceControl-ASQAdapter-Sales");

        var transport = endpointConfiguration.UseTransport<AzureStorageQueueTransport>();
        var connectionString = "UseDevelopmentStorage=true";
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new Exception("Could not read 'AzureStorageQueue.ConnectionString.Endpoints' environment variable. Check sample prerequisites.");
        }

        transport.ConnectionString(connectionString);

        transport.DefaultAccountAlias("storage_account");

        // Required to address https://github.com/Particular/NServiceBus.AzureStorageQueues/issues/308
        transport.AccountRouting().AddAccount("storage_account", connectionString);

        endpointConfiguration.UseSerialization<NewtonsoftSerializer>();

        var recoverability = endpointConfiguration.Recoverability();

        #region NamespaceAlias

        recoverability.Failed(
            customizations: settings =>
            {
                settings.HeaderCustomization(
                    customization: headers =>
                    {
                        headers[AdapterSpecificHeaders.OriginalStorageAccountAlias] = "storage_account";
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
                sendOptions.SetDestination("Samples-ServiceControl-ASQAdapter-Shipping");
                await endpointInstance.Send(shipOrder, sendOptions)
                    .ConfigureAwait(false);
            }
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