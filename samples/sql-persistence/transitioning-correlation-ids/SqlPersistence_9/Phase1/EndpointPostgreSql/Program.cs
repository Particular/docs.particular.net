using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;
using NpgsqlTypes;
using NServiceBus;

Console.Title = "EndpointPostgreSql";

var endpointConfiguration = new EndpointConfiguration("EndpointPostgreSql");

endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());
endpointConfiguration.EnableInstallers();

var connection = "Host=localhost;Username=postgres;Password=yourStrong(!)Password;Database=NsbSamplesSqlPersistenceTransition";

var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();

var dialect = persistence.SqlDialect<SqlDialect.PostgreSql>();

dialect.JsonBParameterModifier(
    modifier: parameter =>
    {
        var npgsqlParameter = (NpgsqlParameter)parameter;
        npgsqlParameter.NpgsqlDbType = NpgsqlDbType.Jsonb;
    });

persistence.ConnectionBuilder(
    () => new NpgsqlConnection(connection));

var subscriptions = persistence.SubscriptionSettings();
subscriptions.CacheFor(TimeSpan.FromMinutes(1));

SqlHelper.EnsureDatabaseExists(connection);

var builder = Host.CreateApplicationBuilder();
builder.Services.AddNServiceBusEndpoint(endpointConfiguration);
var host = builder.Build();
var messageSession = host.Services.GetRequiredService<IMessageSession>();
await host.StartAsync();
await SendMessage(messageSession);

Console.WriteLine("StartOrder Message sent");

Console.WriteLine("Press any key to exit");
Console.ReadKey();

await host.StopAsync();