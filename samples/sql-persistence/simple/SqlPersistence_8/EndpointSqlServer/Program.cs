using System;
using Microsoft.Data.SqlClient;
using NServiceBus;

Console.Title = "EndpointSqlServer";

var endpointConfiguration = new EndpointConfiguration("Samples.SqlPersistence.EndpointSqlServer");
endpointConfiguration.UseSerialization<SystemJsonSerializer>();

#region sqlServerConfig
var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
persistence.SqlDialect<SqlDialect.MsSqlServer>();

// for SqlExpress use Data Source=.\SqlExpress;Initial Catalog=NsbSamplesSqlPersistence;Integrated Security=True;Encrypt=false
var connectionString = "Server=localhost,1433;Initial Catalog=NsbSamplesSqlPersistence;User Id=SA;Password=yourStrong(!)Password;Encrypt=false";

persistence.ConnectionBuilder(() => new SqlConnection(connectionString));
#endregion

var subscriptions = persistence.SubscriptionSettings();
subscriptions.CacheFor(TimeSpan.FromMinutes(1));

SqlHelper.EnsureDatabaseExists(connectionString);

endpointConfiguration.UseTransport(new LearningTransport());
endpointConfiguration.EnableInstallers();

var endpointInstance = await Endpoint.Start(endpointConfiguration);

Console.WriteLine("Press any key to exit");
Console.ReadKey();

await endpointInstance.Stop();
