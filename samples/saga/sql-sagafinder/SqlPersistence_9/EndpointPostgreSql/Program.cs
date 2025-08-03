using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Npgsql;
using NpgsqlTypes;
using NServiceBus;
using NServiceBus.Persistence.Sql;


Console.Title = "PostgreSql";
var builder = Host.CreateApplicationBuilder(args);
var endpointConfiguration = new EndpointConfiguration("Samples.SqlSagaFinder.PostgreSql");
endpointConfiguration.UseTransport(new LearningTransport());
endpointConfiguration.UseSerialization<XmlSerializer>();
endpointConfiguration.SendFailedMessagesTo("error");
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

var endpointInstance = await Endpoint.Start(endpointConfiguration);
var startOrder = new StartOrder
{
    OrderId = "123"
};
await endpointInstance.SendLocal(startOrder);

Console.WriteLine("Press any key to exit");
Console.ReadKey();
await endpointInstance.Stop();
builder.UseNServiceBus(endpointConfiguration);

await builder.Build().RunAsync();