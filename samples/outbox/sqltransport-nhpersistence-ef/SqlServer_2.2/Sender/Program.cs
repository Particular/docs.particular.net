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
        var random = new Random();

        var hibernateConfig = new Configuration();
        hibernateConfig.DataBaseIntegration(
            dataBaseIntegration: configurationProperties =>
            {
                configurationProperties.ConnectionStringName = "NServiceBus/Persistence";
                configurationProperties.Dialect<MsSql2012Dialect>();
            });

        hibernateConfig.SetProperty("default_schema", "sender");

        var busConfiguration = new BusConfiguration();
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.EndpointName("Samples.SQLNHibernateOutboxEF.Sender");

        #region SenderConfiguration

        var transport = busConfiguration.UseTransport<SqlServerTransport>();
        var connectionInfo = EndpointConnectionInfo.For("Samples.SQLNHibernateOutboxEF.Receiver")
            .UseSchema("receiver");
        transport.UseSpecificConnectionInformation(connectionInfo);
        transport.DefaultSchema("sender");

        busConfiguration.UsePersistence<NHibernatePersistence>();

        busConfiguration.EnableOutbox();

        #endregion

        using (var bus = Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("Press enter to send a message");
            Console.WriteLine("Press any key to exit");

            while (true)
            {
                var key = Console.ReadKey();
                Console.WriteLine();

                if (key.Key != ConsoleKey.Enter)
                {
                    return;
                }
                var orderId = new string(Enumerable.Range(0, 4).Select(x => letters[random.Next(letters.Length)]).ToArray());
                var orderSubmitted = new OrderSubmitted
                {
                    OrderId = orderId,
                    Value = random.Next(100)
                };
                bus.Publish(orderSubmitted);
            }
        }
    }
}