using System;
using Messages;
using NServiceBus;
using NServiceBus.Transports.SQLServer;

public class Program
{
    static void Main()
    {
        Console.Title = "Samples.SqlServer.MultiInstanceSender";

        #region SenderConfiguration

        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.SqlServer.MultiInstanceSender");
        var transport = busConfiguration.UseTransport<SqlServerTransport>();
        transport.UseSpecificConnectionInformation(ConnectionInfoProvider.GetConnection);
        transport.ConnectionString(ConnectionInfoProvider.DefaultConnectionString);
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.UsePersistence<InMemoryPersistence>();

        #endregion

        using (var bus = Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("Press <enter> to send a message");
            Console.WriteLine("Press any other key to exit");

            while (true)
            {
                if (Console.ReadKey().Key != ConsoleKey.Enter)
                {
                    return;
                }

                #region SendMessage

                var order = new ClientOrder
                {
                    OrderId = Guid.NewGuid()
                };
                bus.Send("Samples.SqlServer.MultiInstanceReceiver", order);

                #endregion

                Console.WriteLine($"ClientOrder message sent with ID {order.OrderId}");
            }
        }
    }

    #region SenderConnectionProvider

    static class ConnectionInfoProvider
    {
        const string ReceiverConnectionString = @"Data Source=.\SqlExpress;Database=ReceiverCatalog;Integrated Security=True";
        public const string DefaultConnectionString = @"Data Source=.\SqlExpress;Database=SenderCatalog;Integrated Security=True";

        public static ConnectionInfo GetConnection(string transportAddress)
        {
            var connectionString = transportAddress.StartsWith("Samples.SqlServer.MultiInstanceReceiver")
                ? ReceiverConnectionString
                : DefaultConnectionString;

            return ConnectionInfo
                    .Create()
                    .UseConnectionString(connectionString);
        }
    }

    #endregion
}