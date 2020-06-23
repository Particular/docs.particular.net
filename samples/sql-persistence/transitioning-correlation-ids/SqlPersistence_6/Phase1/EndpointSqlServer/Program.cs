using System;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using NServiceBus;

partial class Program
{
    static async Task Main()
    {
        Console.Title = "EndpointSqlServer";

        var endpointConfiguration = new EndpointConfiguration("EndpointSqlServer");

        endpointConfiguration.UseTransport<LearningTransport>();
        endpointConfiguration.EnableInstallers();

        var connection = @"Data Source=.\SqlExpress;Initial Catalog=NsbSamplesSqlPersistenceTransition;Integrated Security=True";
        SqlHelper.EnsureDatabaseExists(connection);
        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        persistence.SqlDialect<SqlDialect.MsSqlServer>();
        persistence.ConnectionBuilder(
            connectionBuilder: () =>
            {
                return new SqlConnection(connection);
            });
        var subscriptions = persistence.SubscriptionSettings();
        subscriptions.CacheFor(TimeSpan.FromMinutes(1));
        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        await SendMessage(endpointInstance)
            .ConfigureAwait(false);
        Console.WriteLine("StartOrder Message sent");

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();

        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}