using System.Data.SqlClient;
using NServiceBus;

var sqlConnectionString = Environment.GetEnvironmentVariable("SqlConnectionString");


var config = new EndpointConfiguration("KubernetesDemo.Publisher");

config.EnableInstallers();

var transport = new SqlServerTransport(sqlConnectionString);
transport.Subscriptions.DisableCaching = true;
config.UseTransport(transport);

var persistence = config.UsePersistence<SqlPersistence>();
persistence.ConnectionBuilder(() => new SqlConnection(sqlConnectionString));
