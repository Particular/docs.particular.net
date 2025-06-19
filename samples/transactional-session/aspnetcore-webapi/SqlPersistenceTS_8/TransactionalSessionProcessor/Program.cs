using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Hosting;
using NServiceBus.TransactionalSession;

Console.Title = "TXSessionProcessor";

var builder = Host.CreateApplicationBuilder(args);

// for SqlExpress use Data Source=.\SqlExpress;Initial Catalog=nservicebus;Integrated Security=True;Encrypt=false
const string ConnectionString = @"Server=localhost,1433;Initial Catalog=nservicebus;User Id=SA;Password=yourStrong(!)Password;Encrypt=false";

var endpointConfiguration = new EndpointConfiguration("TransactionalSessionProcessor");

endpointConfiguration.UseSerialization<SystemJsonSerializer>();

endpointConfiguration.EnableInstallers();

endpointConfiguration.UseTransport(new LearningTransport { TransportTransactionMode = TransportTransactionMode.ReceiveOnly });

var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
persistence.SqlDialect<SqlDialect.MsSqlServer>();
persistence.ConnectionBuilder(() => new SqlConnection(ConnectionString));

#region txsession-nsb-processor-configuration
persistence.EnableTransactionalSession();
endpointConfiguration.EnableOutbox();
#endregion

builder.UseNServiceBus(endpointConfiguration);

await builder.Build().RunAsync();