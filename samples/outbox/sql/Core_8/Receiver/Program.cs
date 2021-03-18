using System;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Transport.SqlServer;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.SQLOutboxEF.Receiver";

        var connection = @"Data Source=.\SqlExpress;Database=NsbSamplesSqlOutbox;Integrated Security=True;Max Pool Size=100";

        var endpointConfiguration = new EndpointConfiguration("Samples.SqlOutbox.Receiver");
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.SendFailedMessagesTo("error");

        #region ReceiverConfiguration

        var transport = new SqlServerTransport(connection)
        {
            DefaultSchema = "receiver",
        };
        transport.SchemaAndCatalog.UseSchemaForQueue("error", "dbo");
        transport.SchemaAndCatalog.UseSchemaForQueue("audit", "dbo");

        var routing = endpointConfiguration.UseTransport(transport);
        routing.UseSchemaForEndpoint("Samples.SqlOutbox.Sender", "sender");

        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        persistence.ConnectionBuilder(
            connectionBuilder: () =>
            {
                return new SqlConnection(connection);
            });
        var dialect = persistence.SqlDialect<SqlDialect.MsSqlServer>();
        dialect.Schema("receiver");
        persistence.TablePrefix("");

        transport.Subscriptions.DisableCaching = true;
        transport.Subscriptions.SubscriptionTableName = new SubscriptionTableName(
            table: "Subscriptions", 
            schema: "dbo");

        endpointConfiguration.EnableOutbox();

        #endregion
        SqlHelper.CreateSchema(connection, "receiver");

        SqlHelper.ExecuteSql(connection, File.ReadAllText("Startup.sql"));

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}