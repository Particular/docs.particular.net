using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Persistence.Sql;
using Oracle.ManagedDataAccess.Client;

partial class Program
{
    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "EndpointOracle";

        var endpointConfiguration = new EndpointConfiguration("EndpointOracle");

        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.EnableInstallers();
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
        persistence.SqlVariant(SqlVariant.Oracle);
        persistence.ConnectionBuilder(
            connectionBuilder: () =>
            {
                return new OracleConnection(connection);
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