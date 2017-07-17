using System;
using NServiceBus;
using NServiceBus.Transports.SQLServer;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.SqlServer.StoreAndForwardSender";
        var random = new Random();
        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.SqlServer.StoreAndForwardSender");

        #region SenderConfiguration

        var transport = busConfiguration.UseTransport<SqlServerTransport>();
        var senderConnection = @"Data Source=.\SqlExpress;Database=sender;Integrated Security=True";
        transport.ConnectionString(senderConnection);
        var receiverConnection = @"Data Source=.\SqlExpress;Database=receiver;Integrated Security=True";
        transport.UseSpecificConnectionInformation(
            EndpointConnectionInfo.For("Samples.SqlServer.StoreAndForwardReceiver")
                .UseConnectionString(receiverConnection)
        );
        busConfiguration.UsePersistence<InMemoryPersistence>();
        var pipeline = busConfiguration.Pipeline;
        pipeline.Register<ForwardBehavior.Registration>();
        pipeline.Register<SendThroughLocalQueueBehavior.Registration>();

        #endregion

        using (var bus = Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("Press enter to publish a message");
            Console.WriteLine("Press any key to exit");
            while (true)
            {
                var key = Console.ReadKey();
                Console.WriteLine();
                if (key.Key != ConsoleKey.Enter)
                {
                    return;
                }
                var orderId = Guid.NewGuid();
                var orderSubmitted = new OrderSubmitted
                {
                    OrderId = orderId,
                    Value = random.Next(100)
                };
                bus.Publish(orderSubmitted);
                Console.WriteLine($"Order {orderId} placed");
            }
        }
    }
}