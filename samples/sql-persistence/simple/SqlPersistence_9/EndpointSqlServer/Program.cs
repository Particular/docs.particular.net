using System;
using EndpointSqlServer;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;

Console.Title = "EndpointSqlServer";

var endpointConfiguration = new EndpointConfiguration("Samples.SqlPersistence.EndpointSqlServer");
endpointConfiguration.UseSerialization<SystemJsonSerializer>();

#region sqlServerConfig

var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
persistence.SqlDialect<SqlDialect.MsSqlServer>();

// for SqlExpress: 
//var connectionString = @"Data Source=.\SqlExpress;Initial Catalog=NsbSamplesSqlPersistence;Integrated Security=True;Encrypt=false";
// for SQL Server:
//var connectionString = @"Server=localhost,1433;Initial Catalog=NsbSamplesSqlPersistence;User Id=SA;Password=yourStrong(!)Password;Encrypt=false";
// for LocalDB:
var connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=NsbSamplesSqlPersistence;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

persistence.ConnectionBuilder(() => new SqlConnection(connectionString));

#endregion

var subscriptions = persistence.SubscriptionSettings();
subscriptions.CacheFor(TimeSpan.FromMinutes(1));

SqlHelper.EnsureDatabaseExists(connectionString);

endpointConfiguration.UseTransport(new LearningTransport());
endpointConfiguration.EnableInstallers();

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
