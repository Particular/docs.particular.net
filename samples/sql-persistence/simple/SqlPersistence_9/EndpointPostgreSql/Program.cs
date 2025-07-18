using System;
using EndpointPostgreSql;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;
using NpgsqlTypes;
using NServiceBus;

Console.Title = "EndpointPostgreSql";

var endpointConfiguration = new EndpointConfiguration("EndpointPostgreSql");
endpointConfiguration.UseSerialization<SystemJsonSerializer>();

#region PostgreSqlConfig

var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
var dialect = persistence.SqlDialect<SqlDialect.PostgreSql>();

var connection = "Host=localhost;Username=postgres;Password=yourStrong(!)Password;Database=NsbSamplesSqlPersistence";

persistence.ConnectionBuilder(() => new NpgsqlConnection(connection));

#endregion

dialect.JsonBParameterModifier(
    modifier: parameter =>
    {
        var npgsqlParameter = (NpgsqlParameter)parameter;
        npgsqlParameter.NpgsqlDbType = NpgsqlDbType.Jsonb;
    });

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