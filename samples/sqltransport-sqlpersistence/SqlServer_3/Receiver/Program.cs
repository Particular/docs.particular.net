using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Persistence.Sql;
using NServiceBus.Transport.SQLServer;

class Program
{
    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Samples.SqlTransportSqlPersistence.Receiver";

        #region ReceiverConfiguration

        var endpointConfiguration = new EndpointConfiguration("Samples.SqlTransportSqlPersistence.Receiver");
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.AuditProcessedMessagesTo("audit");
        endpointConfiguration.EnableInstallers();
        var connectionString = @"Data Source=.\SqlExpress;Database=shared;Integrated Security=True;Min Pool Size=2;Max Pool Size=100";
        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
        transport.ConnectionString(connectionString);
        transport.DefaultSchema("receiver");
        transport.UseSchemaForQueue("error", "dbo");
        transport.UseSchemaForQueue("audit", "dbo");
        transport.UseSchemaForQueue("Samples.SqlTransportSqlPersistence.Sender", "sender");
        transport.Routing().RouteToEndpoint(typeof(OrderAccepted), "Samples.SqlTransportSqlPersistence.Sender");
        transport.Routing().RegisterPublisher(typeof(OrderSubmitted).Assembly, "Samples.SqlTransportSqlPersistence.Sender");

        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        persistence.SqlVariant(SqlVariant.MsSqlServer);
        persistence.ConnectionBuilder(
            connectionBuilder: () =>
            {
                return new SqlConnection(connectionString);
            });
        persistence.Schema("receiver");
        persistence.TablePrefix("");
        var subscriptions = persistence.SubscriptionSettings();
        subscriptions.CacheFor(TimeSpan.FromMinutes(1));

        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        await endpointInstance.Subscribe<OrderSubmitted>().ConfigureAwait(false);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}