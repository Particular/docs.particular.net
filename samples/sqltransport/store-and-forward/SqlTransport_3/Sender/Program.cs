using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Settings;
using NServiceBus.Transport.SQLServer;

#pragma warning disable 618

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.SqlServer.StoreAndForwardSender";
        var random = new Random();
        var endpointConfiguration = new EndpointConfiguration("Samples.SqlServer.StoreAndForwardSender");
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.EnableInstallers();

        #region DelayedRetriesConfig

        var recoverability = endpointConfiguration.Recoverability();
        recoverability.Delayed(
            customizations: delayed =>
            {
                delayed.NumberOfRetries(100);
                delayed.TimeIncrease(TimeSpan.FromSeconds(10));
            });

        #endregion

        recoverability.Immediate(
            customizations: immediate =>
            {
                immediate.NumberOfRetries(0);
            });

        #region SenderConfiguration

        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
        transport.EnableLegacyMultiInstanceMode(GetConnecton);

        var pipeline = endpointConfiguration.Pipeline;
        pipeline.Register(
            stepId: "Forward",
            behavior: new ForwardBehavior(),
            description: "Forwards messages to destinations.");
        pipeline.Register("Store",
            factoryMethod: builder =>
            {
                var localAddress = builder.Build<ReadOnlySettings>().LocalAddress();
                return new SendThroughLocalQueueRoutingToDispatchConnector(localAddress);
            },
            description: "Send messages through local endpoint.");

        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Press enter to publish a message");
        Console.WriteLine("Press any key to exit");
        while (true)
        {
            var key = Console.ReadKey();
            Console.WriteLine();
            if (key.Key != ConsoleKey.Enter)
            {
                break;
            }
            var orderId = Guid.NewGuid();
            var orderSubmitted = new OrderSubmitted
            {
                OrderId = orderId,
                Value = random.Next(100)
            };
            await endpointInstance.Publish(orderSubmitted)
                .ConfigureAwait(false);
            Console.WriteLine($"Order {orderId} placed");
        }
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }

    static async Task<SqlConnection> GetConnecton(string transportAddress)
    {
        string connectionString;
        if (transportAddress.StartsWith("Samples.SqlServer.StoreAndForwardSender") ||
            transportAddress == "error")
        {
            connectionString = Connections.SenderConnectionString;
        }
        else
        {
            connectionString = Connections.ReceiverConnectionString;
        }

        var connection = new SqlConnection(connectionString);

        await connection.OpenAsync()
            .ConfigureAwait(false);

        return connection;
    }
}