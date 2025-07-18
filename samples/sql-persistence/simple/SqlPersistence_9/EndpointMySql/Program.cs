using System;
using EndpointMySql;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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

var builder = Host.CreateApplicationBuilder(args);
builder.UseNServiceBus(endpointConfiguration);
builder.Logging.AddConsole();

// Build and start the host
var host = builder.Build();
await host.StartAsync();

// Get required services
var logger = host.Services.GetRequiredService<ILogger<Program>>();

logger.LogWarning("Press any key to exit");
Console.ReadKey();

await host.StopAsync();