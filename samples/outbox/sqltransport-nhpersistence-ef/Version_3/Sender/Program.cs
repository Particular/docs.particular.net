using System;
using System.Linq;
using System.Threading.Tasks;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NServiceBus;
using NServiceBus.Transports.SQLServer;

class Program
{
    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
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

        EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.EndpointName("Samples.SQLNHibernateOutboxEF.Sender");

        #region SenderConfiguration

        endpointConfiguration
            .UseTransport<SqlServerTransport>()
            .DefaultSchema("sender")
            .UseSpecificSchema(queueName =>
            {
                if (queueName.Equals("error", StringComparison.OrdinalIgnoreCase) || queueName.Equals("audit", StringComparison.OrdinalIgnoreCase))
                {
                    return "dbo";
                }
                return null;
            });

        endpointConfiguration
            .UsePersistence<NHibernatePersistence>();

        endpointConfiguration.EnableOutbox();

        #endregion

        IEndpointInstance endpoint = await Endpoint.Start(endpointConfiguration);

        try
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
                await endpoint.Publish(new OrderSubmitted
                {
                    OrderId = orderId,
                    Value = random.Next(100)
                });
            }
        }
        finally
        {
            await endpoint.Stop();
        }
    }
}