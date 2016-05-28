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
        Console.Title = "Samples.SqlNHibernate.Sender";
        const string letters = "ABCDEFGHIJKLMNOPQRSTUVXYZ";
        var random = new Random();
        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.SqlNHibernate.Sender");
        busConfiguration.EnableInstallers();
        var hibernateConfig = new Configuration();
        hibernateConfig.DataBaseIntegration(x =>
        {
            x.ConnectionStringName = "NServiceBus/Persistence";
            x.Dialect<MsSql2012Dialect>();
        });
        hibernateConfig.SetProperty("default_schema", "sender");

        #region SenderConfiguration

        var transport = busConfiguration.UseTransport<SqlServerTransport>();
        transport.DefaultSchema("sender");
        transport.UseSpecificConnectionInformation(endpoint =>
            {
                if (endpoint == "error" || endpoint == "audit")
                {
                    return ConnectionInfo.Create().UseSchema("dbo");
                }
                if (endpoint == "Samples.SqlNHibernate.Receiver")
                {
                    return ConnectionInfo.Create().UseSchema("receiver");
                }
                return null;
            });
        var persistence = busConfiguration.UsePersistence<NHibernatePersistence>();
        persistence.UseConfiguration(hibernateConfig);

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