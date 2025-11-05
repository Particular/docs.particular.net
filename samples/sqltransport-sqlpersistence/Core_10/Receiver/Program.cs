using System;
using System.IO;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using NServiceBus.Transport.SqlServer;

Console.Title = "Receiver";
var builder = Host.CreateApplicationBuilder(args);

#region ReceiverConfiguration

var endpointConfiguration = new EndpointConfiguration("Samples.Sql.Receiver");
endpointConfiguration.SendFailedMessagesTo("error");
endpointConfiguration.AuditProcessedMessagesTo("audit");
endpointConfiguration.EnableInstallers();
// for SqlExpress use Data Source=.\SqlExpress;Initial Catalog=NsbSamplesSql;Integrated Security=True;Max Pool Size=100;Encrypt=false
var connectionString = @"Server=localhost,1433;Initial Catalog=NsbSamplesSql;User Id=SA;Password=yourStrong(!)Password;Max Pool Size=100;Encrypt=false";

var transport = new SqlServerTransport(connectionString)
{
    DefaultSchema = "receiver",
    TransportTransactionMode = TransportTransactionMode.SendsAtomicWithReceive,
    Subscriptions =
            {
                CacheInvalidationPeriod = TimeSpan.FromMinutes(1),
                SubscriptionTableName = new SubscriptionTableName(table: "Subscriptions", schema: "dbo")
            }
};

transport.SchemaAndCatalog.UseSchemaForQueue("error", "dbo");
transport.SchemaAndCatalog.UseSchemaForQueue("audit", "dbo");
transport.SchemaAndCatalog.UseSchemaForQueue("Samples.Sql.Sender", "sender");

endpointConfiguration.UseSerialization<SystemJsonSerializer>();
var routing = endpointConfiguration.UseTransport(transport);
routing.RouteToEndpoint(typeof(OrderAccepted), "Samples.Sql.Sender");

var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
var dialect = persistence.SqlDialect<SqlDialect.MsSqlServer>();
dialect.Schema("receiver");
persistence.ConnectionBuilder(() => new SqlConnection(connectionString));
persistence.TablePrefix("");

#endregion

await SqlHelper.CreateSchema(connectionString, "receiver");
var allText = File.ReadAllText("Startup.sql");
await SqlHelper.ExecuteSql(connectionString, allText);

builder.UseNServiceBus(endpointConfiguration);

await builder.Build().RunAsync();
