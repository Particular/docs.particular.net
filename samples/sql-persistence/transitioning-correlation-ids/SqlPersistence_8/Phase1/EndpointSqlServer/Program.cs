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

        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        endpointConfiguration.UseTransport(new LearningTransport());
        endpointConfiguration.EnableInstallers();

        // for SqlExpress use Data Source=.\SqlExpress;Initial Catalog=NsbSamplesSqlPersistenceTransition;Integrated Security=True;Encrypt=false
        var connectionString = "Server=localhost,1433;Initial Catalog=NsbSamplesSqlPersistenceTransition;User Id=SA;Password=yourStrong(!)Password;Encrypt=false";

        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        persistence.SqlDialect<SqlDialect.MsSqlServer>();

        persistence.ConnectionBuilder(
            () => new SqlConnection(connectionString));

        var subscriptions = persistence.SubscriptionSettings();
        subscriptions.CacheFor(TimeSpan.FromMinutes(1));

        SqlHelper.EnsureDatabaseExists(connectionString);

        var endpointInstance = await Endpoint.Start(endpointConfiguration);

        await SendMessage(endpointInstance);

        Console.WriteLine("StartOrder Message sent");

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();

        await endpointInstance.Stop();
    }
}