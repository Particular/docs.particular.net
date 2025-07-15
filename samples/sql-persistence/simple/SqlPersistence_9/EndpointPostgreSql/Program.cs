using System;
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

var endpointInstance = await Endpoint.Start(endpointConfiguration);

Console.WriteLine("Press any key to exit");
Console.ReadKey();

await endpointInstance.Stop();
