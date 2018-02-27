using System;
using System.Linq;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.ServiceControl.MixedTransportAdapter.Shipping";
        const string letters = "ABCDEFGHIJKLMNOPQRSTUVXYZ";
        var random = new Random();
        var endpointConfiguration = new EndpointConfiguration(
            "Samples.ServiceControl.MixedTransportAdapter.Shipping");

        var connection = @"Data Source=.\SqlExpress;Initial Catalog=shipping;Integrated Security=True;Max Pool Size=100;Min Pool Size=10";

        var transport = endpointConfiguration.UseTransport<SqlServerTransport>()
            .ConnectionString(connection);

        SqlHelper.EnsureDatabaseExists(connection);

        endpointConfiguration.UsePersistence<InMemoryPersistence>();

        var chaos = new ChaosGenerator();
        endpointConfiguration.RegisterComponents(
            registration: components =>
            {
                components.ConfigureComponent(() => chaos, DependencyLifecycle.SingleInstance);
            });

        endpointConfiguration.Recoverability()
            .Immediate(immediate => immediate.NumberOfRetries(0))
            .Delayed(delayed => delayed.NumberOfRetries(0));

        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.AuditProcessedMessagesTo("audit");
        endpointConfiguration.SendHeartbeatTo("Particular.ServiceControl");
        endpointConfiguration.EnableInstallers();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Press enter to exit");
        Console.WriteLine("Press 'o' to send a message");
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
                var id = new string(Enumerable.Range(0, 4).Select(x => letters[random.Next(letters.Length)]).ToArray());
                var message = new ShippingMessage
                {
                    Id = id,
                };
                await endpointInstance.SendLocal(message)
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