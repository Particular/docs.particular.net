using System;
using System.Threading.Tasks;
using NServiceBus;
using Oracle.ManagedDataAccess.Client;

internal class Program
{
    static async Task Main()
    {
        Console.Title = "EndpointOracle";

        #region OracleConfig

        var endpointConfiguration = new EndpointConfiguration("EndpointOracle");

        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        var password = Environment.GetEnvironmentVariable("OraclePassword");
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new Exception("Could not extract 'OraclePassword' from Environment variables.");
        }
        var username = Environment.GetEnvironmentVariable("OracleUserName");
        if (string.IsNullOrWhiteSpace(username))
        {
            throw new Exception("Could not extract 'OracleUserName' from Environment variables.");
        }
        var connection = $"Data Source=localhost;User Id={username}; Password={password}; Enlist=false";
        persistence.SqlDialect < SqlDialect.Oracle>();
        persistence.ConnectionBuilder(
            connectionBuilder: () =>
            {
                return new OracleConnection(connection);
            });
        var subscriptions = persistence.SubscriptionSettings();
        subscriptions.CacheFor(TimeSpan.FromMinutes(1));

        #endregion

        endpointConfiguration.UseTransport<LearningTransport>();
        endpointConfiguration.EnableInstallers();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();

        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}