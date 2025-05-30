using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Hosting;
using NServiceBus.TransactionalSession;

Console.Title = "Processor";

var builder = Host.CreateApplicationBuilder(args);

const string ConnectionString = @"";

var endpointConfiguration = new EndpointConfiguration("Processor");

endpointConfiguration.UseSerialization<SystemJsonSerializer>();

endpointConfiguration.EnableInstallers();

endpointConfiguration.UseTransport(new SqlServerTransport(ConnectionString) { TransportTransactionMode = TransportTransactionMode.ReceiveOnly });

var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
persistence.SqlDialect<SqlDialect.MsSqlServer>();
persistence.ConnectionBuilder(() => new SqlConnection(ConnectionString));
persistence.EnableTransactionalSession();
endpointConfiguration.EnableOutbox();

builder.UseNServiceBus(endpointConfiguration);

await builder.Build().RunAsync();