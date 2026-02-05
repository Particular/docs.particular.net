using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;
using NpgsqlTypes;
using NServiceBus;

Console.Title = "PostgreSql";
var builder = Host.CreateApplicationBuilder(args);
var endpointConfiguration = new EndpointConfiguration("Samples.SqlSagaFinder.PostgreSql");
endpointConfiguration.UseTransport(new LearningTransport());
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.EnableInstallers();
#region PostgreSqlConfig

var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
var password = Environment.GetEnvironmentVariable("PostgreSqlPassword");
if (string.IsNullOrWhiteSpace(password))
{
    throw new Exception("Could not extract 'PostgreSqlPassword' from Environment variables.");
}
var username = Environment.GetEnvironmentVariable("PostgreSqlUserName");
if (string.IsNullOrWhiteSpace(username))
{
    throw new Exception("Could not extract 'PostgreSqlUserName' from Environment variables.");
}
var connection = $"Host=localhost;Username={username};Password={password};Database=NsbSamplesSqlSagaFinder";
persistence.TablePrefix("Finder");
var dialect = persistence.SqlDialect<SqlDialect.PostgreSql>();
dialect.JsonBParameterModifier(
    modifier: parameter =>
    {
        var npgsqlParameter = (NpgsqlParameter)parameter;
        npgsqlParameter.NpgsqlDbType = NpgsqlDbType.Jsonb;
    });
persistence.ConnectionBuilder(
    connectionBuilder: () =>
    {
        return new NpgsqlConnection(connection);
    });

var subscriptions = persistence.SubscriptionSettings();
subscriptions.CacheFor(TimeSpan.FromMinutes(1));

#endregion

builder.UseNServiceBus(endpointConfiguration);

var host = builder.Build();

await host.StartAsync();

var messageSession = host.Services.GetRequiredService<IMessageSession>();

Console.WriteLine("Press 'enter' to send a message");
while (true)
{
    var key = Console.ReadKey();
    Console.WriteLine();

    if (key.Key != ConsoleKey.Enter)
    {
        break;
    }

    var startOrder = new StartOrder
    {
        OrderId = "123"
    };
    await messageSession.SendLocal(startOrder);

    Console.WriteLine($"StartOrder sent: {startOrder.OrderId}");
}

await host.StopAsync();