using System;
using System.Linq;
using NServiceBus;
using NServiceBus.Transports.SQLServer;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NServiceBus.Persistence;

class Program
{
    static void Main()
    {
        const string letters = "ABCDEFGHIJKLMNOPQRSTUVXYZ";
        Random random = new Random();
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EnableInstallers();
        Configuration hibernateConfig = new Configuration();
        hibernateConfig.DataBaseIntegration(x =>
        {
            x.ConnectionStringName = "NServiceBus/Persistence";
            x.Dialect<MsSql2012Dialect>();
        });
        hibernateConfig.SetProperty("default_schema", "sender");

        #region SenderConfiguration

        busConfiguration.UseTransport<SqlServerTransport>()
            .DefaultSchema("sender")
            .UseSpecificConnectionInformation(
                EndpointConnectionInfo.For("receiver")
                    .UseSchema("receiver"));
        busConfiguration.UsePersistence<NHibernatePersistence>()
            .UseConfiguration(hibernateConfig);

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