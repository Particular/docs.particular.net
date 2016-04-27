using System;
using Messages;
using NServiceBus;
using NServiceBus.Config;
using NServiceBus.Config.ConfigurationSource;

public class Program
{
    static void Main()
    {
        Console.Title = "Samples.SqlServer.MultiInstanceSender";

        #region SenderConfiguration

        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.SqlServer.MultiInstanceSender");
        var transport = busConfiguration.UseTransport<SqlServerTransport>();
        transport.UseSpecificConnectionInformation(ConnectionProvider.GetConnection);
        transport.ConnectionString(ConnectionProvider.SenderConnectionString);
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.UsePersistence<InMemoryPersistence>();

        #endregion

        using (IBus bus = Bus.Create(busConfiguration).Start())
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

                ClientOrder order = new ClientOrder
                {
                    OrderId = Guid.NewGuid()
                };

                bus.Send("Samples.SqlServer.MultiInstanceReceiver", order);

                #endregion

                Console.WriteLine("ClientOrder message sent with ID {0}", order.OrderId);
            }
        }
    }

    class ProvideConfiguration : IProvideConfiguration<MessageForwardingInCaseOfFaultConfig>
    {
        public MessageForwardingInCaseOfFaultConfig GetConfiguration()
        {
            return new MessageForwardingInCaseOfFaultConfig
            {
                ErrorQueue = "error"
            };
        }
    }
}