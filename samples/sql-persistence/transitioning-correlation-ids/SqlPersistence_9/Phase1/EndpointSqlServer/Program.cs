using System;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;

Console.Title = "EndpointSqlServer";

var endpointConfiguration = new EndpointConfiguration("EndpointSqlServer");

endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());
endpointConfiguration.EnableInstallers();

// for SqlExpress use Data Source=.\SqlExpress;Initial Catalog=NsbSamplesSqlPersistenceTransition;Integrated Security=True;Encrypt=false
var connectionString = "Server=localhost,1433;Initial Catalog=NsbSamplesSqlPersistenceTransition;User Id=SA;Password=yourStrong(!)Password;Encrypt=false";

var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
persistence.SqlDialect<SqlDialect.MsSqlServer>();
persistence.ConnectionBuilder(
    () => new SqlConnection(connectionString));

var subscriptions = persistence.SubscriptionSettings();
subscriptions.CacheFor(TimeSpan.FromMinutes(1));

SqlHelper.EnsureDatabaseExists(connectionString);

var builder = Host.CreateApplicationBuilder();
builder.Services.AddNServiceBusEndpoint(endpointConfiguration);
using var host = builder.Build();
var messageSession = host.Services.GetRequiredService<IMessageSession>();
await host.StartAsync();
await SendMessage(messageSession);

Console.WriteLine("StartOrder Message sent");
Console.WriteLine("Press any key to exit");
Console.ReadKey();

await host.StopAsync();