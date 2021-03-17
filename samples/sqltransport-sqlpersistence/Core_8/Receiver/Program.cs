using System;
using Microsoft.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Transport.SqlServer;

public static class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.Sql.Receiver";

        #region ReceiverConfiguration

        var endpointConfiguration = new EndpointConfiguration("Samples.Sql.Receiver");
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.AuditProcessedMessagesTo("audit");
        endpointConfiguration.EnableInstallers();
        var connection = @"Data Source=.\SqlExpress;Database=NsbSamplesSql;Integrated Security=True;Max Pool Size=100";

        var transport = new SqlServerTransport(connection)
        {
            DefaultSchema = "receiver",
            TransportTransactionMode = TransportTransactionMode.SendsAtomicWithReceive,
            Subscriptions = 
            {
                CacheInvalidationPeriod = TimeSpan.FromMinutes(1),
                SubscriptionTableName = new SubscriptionTableName(
                    table: "Subscriptions", schema: "dbo")
            }
        };

        transport.SchemaAndCatalog.UseSchemaForQueue("error", "dbo");
        transport.SchemaAndCatalog.UseSchemaForQueue("audit", "dbo");
        transport.SchemaAndCatalog.UseSchemaForQueue("Samples.Sql.Sender", "sender");

        var routing = endpointConfiguration.UseTransport(transport);
        routing.RouteToEndpoint(typeof(OrderAccepted), "Samples.Sql.Sender");

        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        var dialect = persistence.SqlDialect<SqlDialect.MsSqlServer>();
        dialect.Schema("receiver");
        persistence.ConnectionBuilder(
            connectionBuilder: () =>
            {
                return new SqlConnection(connection);
            });
        persistence.TablePrefix("");

        #endregion

        SqlHelper.CreateSchema(connection, "receiver");
        var allText = File.ReadAllText("Startup.sql");
        SqlHelper.ExecuteSql(connection, allText);
        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}