using System;
using System.IO;
using Microsoft.Data.SqlClient;
using NServiceBus;

Console.Title = "Samples.Sql.Receiver";

#region ReceiverConfiguration

var endpointConfiguration = new EndpointConfiguration("Samples.Sql.Receiver");
endpointConfiguration.SendFailedMessagesTo("error");
endpointConfiguration.AuditProcessedMessagesTo("audit");
endpointConfiguration.EnableInstallers();
// for SqlExpress use Data Source=.\SqlExpress;Initial Catalog=NsbSamplesSql;Integrated Security=True;Max Pool Size=100;Encrypt=false
var connectionString = @"Server=localhost,1433;Initial Catalog=NsbSamplesSql;User Id=SA;Password=yourStrong(!)Password;Max Pool Size=100;Encrypt=false";

var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
transport.ConnectionString(connectionString);
transport.DefaultSchema("receiver");
transport.UseSchemaForQueue("error", "dbo");
transport.UseSchemaForQueue("audit", "dbo");
transport.UseSchemaForQueue("Samples.Sql.Sender", "sender");
transport.Transactions(TransportTransactionMode.SendsAtomicWithReceive);

var subscriptions = transport.SubscriptionSettings();
subscriptions.CacheSubscriptionInformationFor(TimeSpan.FromMinutes(1));
subscriptions.SubscriptionTableName(tableName: "Subscriptions", schemaName: "dbo");

var routing = transport.Routing();
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
var endpointInstance = await Endpoint.Start(endpointConfiguration);
Console.WriteLine("Press any key to exit");
Console.ReadKey();

await endpointInstance.Stop();