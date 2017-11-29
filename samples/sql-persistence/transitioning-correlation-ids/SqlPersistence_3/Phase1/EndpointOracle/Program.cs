using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Persistence.Sql;
using Oracle.ManagedDataAccess.Client;

partial class Program
{
    static async Task Main()
    {
<<<<<<< HEAD
        //required to prevent possible occurrence of .NET Core issue https://github.com/dotnet/coreclr/issues/12668
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

=======
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
>>>>>>> parent of ac6888549... Add workaround for .NET Core bug (#3187)
        Console.Title = "EndpointOracle";

        var endpointConfiguration = new EndpointConfiguration("EndpointOracle");

        endpointConfiguration.UseTransport<LearningTransport>();
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
        persistence.SqlDialect<SqlDialect.Oracle>();
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