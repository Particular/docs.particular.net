using System;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using NServiceBus;

partial class Program
{
    static async Task Main()
    {
        Console.Title = "EndpointMySql";

        var endpointConfiguration = new EndpointConfiguration("EndpointMySql");

        endpointConfiguration.UseTransport<LearningTransport>();
        endpointConfiguration.EnableInstallers();
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