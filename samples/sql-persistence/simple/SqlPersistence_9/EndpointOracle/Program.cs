using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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