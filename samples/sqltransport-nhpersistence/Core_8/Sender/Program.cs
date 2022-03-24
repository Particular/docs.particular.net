using System;
using System.Threading.Tasks;
using NServiceBus;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NServiceBus.Persistence;
using NServiceBus.Transport.SqlServer;

class Program
{
    static Random random;

    static async Task Main()
    {
        random = new Random();
        Console.Title = "Samples.SqlNHibernate.Sender";
        var connection = @"Data Source=.\SqlExpress;Database=NsbSamplesSqlNHibernate;Integrated Security=True;Max Pool Size=100";
        var endpointConfiguration = new EndpointConfiguration("Samples.SqlNHibernate.Sender");
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.EnableInstallers();

        var hibernateConfig = new Configuration();
        hibernateConfig.DataBaseIntegration(x =>
        {
            x.ConnectionString = connection;
            x.Dialect<MsSql2012Dialect>();
        });
        hibernateConfig.SetProperty("default_schema", "sender");

        #region SenderConfiguration

        var transport = new SqlServerTransport(connection)
        {
            DefaultSchema = "sender"
        };
        transport.SchemaAndCatalog.UseSchemaForQueue("error", "dbo");
        transport.SchemaAndCatalog.UseSchemaForQueue("audit", "dbo");
        transport.Subscriptions.SubscriptionTableName = new SubscriptionTableName("Subscriptions", "dbo");

        var persistence = endpointConfiguration.UsePersistence<NHibernatePersistence>();
        persistence.UseConfiguration(hibernateConfig);

        #endregion

        var routing = endpointConfiguration.UseTransport(transport);
        routing.RouteToEndpoint(typeof(OrderAccepted), "Samples.SqlNHibernate.Sender");


        SqlHelper.CreateSchema(connection, "sender");

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Press enter to send a message");
        Console.WriteLine("Press any key to exit");

        while (true)
        {
            var key = Console.ReadKey();
            Console.WriteLine();

            if (key.Key != ConsoleKey.Enter)
            {
                break;
            }

            var orderSubmitted = new OrderSubmitted
            {
                OrderId = Guid.NewGuid(),
                Value = random.Next(100)
            };
            await endpointInstance.Publish(orderSubmitted)
                .ConfigureAwait(false);
            Console.WriteLine("Published OrderSubmitted message");
        }
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}