using System;
using System.Linq;
using System.Threading.Tasks;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NServiceBus;
using NServiceBus.Persistence;
using NServiceBus.Transport.SQLServer;

class Program
{
   
    static async Task Main()
    {
        var connection = @"Data Source=.\SqlExpress;Database=NsbSamplesEfUow;Integrated Security=True";
        Console.Title = "Samples.EntityFrameworkUnitOfWork.Sender";
        var random = new Random();


        var endpointConfiguration = new EndpointConfiguration("Samples.EntityFrameworkUnitOfWork.Sender");
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.SendFailedMessagesTo("error");

        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
        transport.ConnectionString(connection);
        transport.DefaultSchema("sender");
        transport.UseSchemaForQueue("error", "dbo");
        transport.UseSchemaForQueue("audit", "dbo");

        var persistence = endpointConfiguration.UsePersistence<NHibernatePersistence>();
        var hibernateConfig = new Configuration();
        hibernateConfig.DataBaseIntegration(x =>
        {
            x.ConnectionString = connection;
            x.Dialect<MsSql2012Dialect>();
        });

        hibernateConfig.SetProperty("default_schema", "sender");
        persistence.UseConfiguration(hibernateConfig);

        SqlHelper.CreateSchema(connection, "sender");
        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        const string letters = "ABCDEFGHIJKLMNOPQRSTUVXYZ";
        var locations = new[] { "London", "Paris", "Oslo", "Madrid" };

        try
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
                var shipTo = locations[random.Next(locations.Length)];
                var orderSubmitted = new OrderSubmitted
                {
                    OrderId = orderId,
                    Value = random.Next(100),
                    ShipTo = shipTo
                };
                await endpointInstance.Publish(orderSubmitted)
                    .ConfigureAwait(false);
            }
        }
        finally
        {
            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }
    }
}