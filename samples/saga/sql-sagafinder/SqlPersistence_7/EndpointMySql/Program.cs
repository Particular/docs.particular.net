using System;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using NServiceBus;
using NServiceBus.Persistence.Sql;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.SqlSagaFinder.MySql";
        var endpointConfiguration = new EndpointConfiguration("Samples.SqlSagaFinder.MySql");
        endpointConfiguration.UseTransport(new LearningTransport());
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.EnableInstallers();
        #region MySqlConfig

        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        var connection = $"server=localhost;user=MySqlUserName;database=sqlpersistencesample;port=3306;password=yourStrong(!)Password;AllowUserVariables=True;AutoEnlist=false";
        persistence.SqlDialect<SqlDialect.MySql>();
        persistence.ConnectionBuilder(
            connectionBuilder: () =>
            {
                return new MySqlConnection(connection);
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