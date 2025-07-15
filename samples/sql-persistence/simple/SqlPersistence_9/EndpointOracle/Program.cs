using System;
using NServiceBus;
using Oracle.ManagedDataAccess.Client;


Console.Title = "EndpointOracle";

var endpointConfiguration = new EndpointConfiguration("EndpointOracle");
endpointConfiguration.UseSerialization<SystemJsonSerializer>();

#region OracleConfig
var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
persistence.SqlDialect<SqlDialect.Oracle>();

var connection = "Data Source=localhost;User Id=SYSTEM; Password=yourStrong(!)Password; Enlist=false";

persistence.ConnectionBuilder(() => new OracleConnection(connection));
#endregion

var subscriptions = persistence.SubscriptionSettings();
subscriptions.CacheFor(TimeSpan.FromMinutes(1));

endpointConfiguration.UseTransport(new LearningTransport());
endpointConfiguration.EnableInstallers();

var endpointInstance = await Endpoint.Start(endpointConfiguration);

Console.WriteLine("Press any key to exit");
Console.ReadKey();

await endpointInstance.Stop();
