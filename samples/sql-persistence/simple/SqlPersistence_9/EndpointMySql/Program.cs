using System;
using MySql.Data.MySqlClient;
using NServiceBus;

Console.Title = "EndpointMySql";

var endpointConfiguration = new EndpointConfiguration("Samples.SqlPersistence.EndpointMySql");
endpointConfiguration.UseSerialization<SystemJsonSerializer>();

#region MySqlConfig
var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
persistence.SqlDialect<SqlDialect.MySql>();

var connection = "server=localhost;user=root;database=sqlpersistencesample;port=3306;password=yourStrong(!)Password;AllowUserVariables=True;AutoEnlist=false";

persistence.ConnectionBuilder(() => new MySqlConnection(connection));
#endregion

var subscriptions = persistence.SubscriptionSettings();
subscriptions.CacheFor(TimeSpan.FromMinutes(1));

endpointConfiguration.UseTransport(new LearningTransport());
endpointConfiguration.EnableInstallers();

SqlHelper.EnsureDatabaseExists(connection);

var endpointInstance = await Endpoint.Start(endpointConfiguration);

Console.WriteLine("Press any key to exit");
Console.ReadKey();

await endpointInstance.Stop();
