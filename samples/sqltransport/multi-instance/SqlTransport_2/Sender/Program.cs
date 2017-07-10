using System;
using Messages;
using NServiceBus;

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

}