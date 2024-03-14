using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.SqlPersistence.InjectingServices";

        var endpointConfiguration = new EndpointConfiguration("Samples.SqlPersistence.InjectingServices");
        endpointConfiguration.EnableInstallers();

        // for SqlExpress use Data Source=.\SqlExpress;Initial Catalog=NsbSamplesInjectedServices;Integrated Security=True;Encrypt=false
        var connectionString = @"Server=localhost,1433;Initial Catalog=NsbSamplesInjectedServices;User Id=SA;Password=yourStrong(!)Password;Encrypt=false";

        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
        transport.ConnectionString(connectionString);
        transport.Transactions(TransportTransactionMode.SendsAtomicWithReceive);

        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        persistence.SqlDialect<SqlDialect.MsSqlServer>();
        persistence.ConnectionBuilder(
            connectionBuilder: () => new SqlConnection(connectionString));
        var subscriptions = persistence.SubscriptionSettings();
        subscriptions.CacheFor(TimeSpan.FromMinutes(1));

        #region BehaviorConfig

        endpointConfiguration.Pipeline.Register(typeof(SqlConnectionBehavior),
            "Forwards the NServiceBus SqlConnection/SqlTransaction to data services injected into message handlers.");

        #endregion

        #region DependencyInjectionConfig

        var containerSettings = endpointConfiguration.UseContainer(new DefaultServiceProviderFactory());
        var services = containerSettings.ServiceCollection;
        services.AddScoped<ConnectionHolder>();
        services.AddScoped<IDataService, DataService>();

        #endregion

        SqlHelper.EnsureDatabaseExists(connectionString);

        var endpointInstance = await Endpoint.Start(endpointConfiguration);

        Console.WriteLine("Press S to send a message, or Enter to exit");

        await RunLoop(endpointInstance);

        await endpointInstance.Stop();
    }

    static async Task RunLoop(IMessageSession messageSession)
    {
        while (true)
        {
            var key = Console.ReadKey(true);
            switch (key.Key)
            {
                case ConsoleKey.Enter:
                    return;

                case ConsoleKey.S:
                    await messageSession.SendLocal(new TestMsg { Id = Guid.NewGuid() });
                    break;
            }
        }
    }
}