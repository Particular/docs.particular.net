using System;
using System.Threading.Tasks;
using Npgsql;
using NpgsqlTypes;
using NServiceBus;
using NServiceBus.Persistence.Sql;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.SqlSagaFinder.PostgreSql";
        var endpointConfiguration = new EndpointConfiguration("Samples.SqlSagaFinder.PostgreSql");
        endpointConfiguration.UseTransport<LearningTransport>();
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

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        var startOrder = new StartOrder
        {
            OrderId = "123"
        };
        await endpointInstance.SendLocal(startOrder)
            .ConfigureAwait(false);

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}