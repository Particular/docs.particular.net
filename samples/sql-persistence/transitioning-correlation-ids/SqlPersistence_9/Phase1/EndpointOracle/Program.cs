using System;
using System.Threading.Tasks;
using NServiceBus;
using Oracle.ManagedDataAccess.Client;

partial class Program
{
    static async Task Main()
    {
        Console.Title = "EndpointOracle";

        var endpointConfiguration = new EndpointConfiguration("EndpointOracle");

        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        endpointConfiguration.UseTransport(new LearningTransport());
        endpointConfiguration.EnableInstallers();

        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();

        var connection = "Data Source=localhost;User Id=SYSTEM; Password=yourStrong(!)Password; Enlist=false";

        persistence.SqlDialect<SqlDialect.Oracle>();
        persistence.ConnectionBuilder(() => new OracleConnection(connection));

        var subscriptions = persistence.SubscriptionSettings();
        subscriptions.CacheFor(TimeSpan.FromMinutes(1));

        var endpointInstance = await Endpoint.Start(endpointConfiguration);

        await SendMessage(endpointInstance);

        Console.WriteLine("StartOrder Message sent");

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();

        await endpointInstance.Stop();
    }
}
