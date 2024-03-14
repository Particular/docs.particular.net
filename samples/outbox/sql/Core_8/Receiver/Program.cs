using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using NServiceBus;
using NServiceBus.Transport.SqlServer;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.SQLOutboxEF.Receiver";

        // for SqlExpress use Data Source=.\SqlExpress;Initial Catalog=NsbSamplesSqlOutbox;Integrated Security=True;Max Pool Size=100;Encrypt=false
        var connectionString = @"Server=localhost,1433;Initial Catalog=NsbSamplesSqlOutbox;User Id=SA;Password=yourStrong(!)Password;Max Pool Size=100;Encrypt=false";

        var endpointConfiguration = new EndpointConfiguration("Samples.SqlOutbox.Receiver");
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.SendFailedMessagesTo("error");

        #region ReceiverConfiguration

        var transport = new SqlServerTransport(connectionString)
        {
            DefaultSchema = "receiver",
            TransportTransactionMode = TransportTransactionMode.ReceiveOnly
        };
        transport.SchemaAndCatalog.UseSchemaForQueue("error", "dbo");
        transport.SchemaAndCatalog.UseSchemaForQueue("audit", "dbo");

        var routing = endpointConfiguration.UseTransport(transport);
        routing.UseSchemaForEndpoint("Samples.SqlOutbox.Sender", "sender");

        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        persistence.ConnectionBuilder(
            connectionBuilder: () =>
            {
                return new SqlConnection(connectionString);
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
        SqlHelper.CreateSchema(connectionString, "receiver");

        SqlHelper.ExecuteSql(connectionString, File.ReadAllText("Startup.sql"));

        var endpointInstance = await Endpoint.Start(endpointConfiguration);

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop();
    }
}