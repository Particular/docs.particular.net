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
        endpointConfiguration.UseTransport<LearningTransport>();
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.EnableInstallers();
        #region MySqlConfig

        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        var password = Environment.GetEnvironmentVariable("MySqlPassword");
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new Exception("Could not extract 'MySqlPassword' from Environment variables.");
        }
        var username = Environment.GetEnvironmentVariable("MySqlUserName");
        if (string.IsNullOrWhiteSpace(username))
        {
            throw new Exception("Could not extract 'MySqlUserName' from Environment variables.");
        }
        var connection = $"server=localhost;user={username};database=sqlpersistencesample;port=3306;password={password};AllowUserVariables=True;AutoEnlist=false";
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