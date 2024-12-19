using System;
using System.Threading.Tasks;
using Npgsql;
using NpgsqlTypes;
using NServiceBus;

partial class Program
{
    static async Task Main()
    {
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

        var endpointInstance = await Endpoint.Start(endpointConfiguration);

        await SendMessage(endpointInstance);

        Console.WriteLine("StartOrder Message sent");

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();

        await endpointInstance.Stop();
    }
}