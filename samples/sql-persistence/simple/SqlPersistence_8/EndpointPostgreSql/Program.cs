using System;
using Npgsql;
using NpgsqlTypes;
using NServiceBus;


Console.Title = "EndpointPostgreSql";

#region PostgreSqlConfig

var endpointConfiguration = new EndpointConfiguration("EndpointPostgreSql");
endpointConfiguration.UseSerialization<SystemJsonSerializer>();


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

var connection = $"Host=localhost;Username={username};Password={password};Database=NsbSamplesSqlPersistence";

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

endpointConfiguration.UseTransport(new LearningTransport());
endpointConfiguration.EnableInstallers();

SqlHelper.EnsureDatabaseExists(connection);

var endpointInstance = await Endpoint.Start(endpointConfiguration)
    .ConfigureAwait(false);

Console.WriteLine("Press any key to exit");
Console.ReadKey();

await endpointInstance.Stop()
    .ConfigureAwait(false);
