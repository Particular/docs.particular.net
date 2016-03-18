using System;
using System.Linq;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NServiceBus;
using NServiceBus.Transports.SQLServer;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.SQLNHibernateOutboxEF.Sender";
        const string letters = "ABCDEFGHIJKLMNOPQRSTUVXYZ";
        Random random = new Random();

        Configuration hibernateConfig = new Configuration();
        hibernateConfig.DataBaseIntegration(x =>
        {
            x.ConnectionStringName = "NServiceBus/Persistence";
            x.Dialect<MsSql2012Dialect>();
        });

        hibernateConfig.SetProperty("default_schema", "sender");

        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.EndpointName("Samples.SQLNHibernateOutboxEF.Sender");

        #region SenderConfiguration

        busConfiguration
            .UseTransport<SqlServerTransport>()
            .UseSpecificConnectionInformation(
                EndpointConnectionInfo.For("Samples.SQLNHibernateOutboxEF.Receiver").UseSchema("receiver"))
            .DefaultSchema("sender");

        busConfiguration
            .UsePersistence<NHibernatePersistence>();

        busConfiguration.EnableOutbox();

        #endregion

        using (IBus bus = Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("Press enter to send a message");
            Console.WriteLine("Press any key to exit");

            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey();
                Console.WriteLine();

                if (key.Key != ConsoleKey.Enter)
                {
                    return;
                }
                string orderId = new string(Enumerable.Range(0, 4).Select(x => letters[random.Next(letters.Length)]).ToArray());
                bus.Publish(new OrderSubmitted
                {
                    OrderId = orderId,
                    Value = random.Next(100)
                });

            }
        }
    }
}