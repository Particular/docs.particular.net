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
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();

        endpointConfiguration.UseTransport(new LearningTransport());
        endpointConfiguration.EnableInstallers();

        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();

        var connection = "server=localhost;user=root;database=sqlpersistencesampletransition;port=3306;password=yourStrong(!)Password;AllowUserVariables=True;AutoEnlist=false";

        persistence.SqlDialect<SqlDialect.MySql>();
        persistence.ConnectionBuilder(
            () => new MySqlConnection(connection));

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