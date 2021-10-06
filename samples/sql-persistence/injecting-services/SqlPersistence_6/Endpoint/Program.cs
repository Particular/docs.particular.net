using Microsoft.Data.SqlClient;
using NServiceBus;
using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.SqlPersistence.InjectingServices";

        #region SqlServerConfig

        var endpointConfiguration = new EndpointConfiguration("Samples.SqlPersistence.InjectingServices");
        endpointConfiguration.EnableInstallers();

        var connectionString = @"Data Source=.\SqlExpress;Initial Catalog=NsbSamplesSqlPersistence;Integrated Security=True";

        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
        transport.ConnectionString(connectionString);
        transport.Transactions(TransportTransactionMode.SendsAtomicWithReceive);

        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        persistence.SqlDialect<SqlDialect.MsSqlServer>();
        persistence.ConnectionBuilder(
            connectionBuilder: () =>
            {
                return new SqlConnection(connectionString);
            });
        var subscriptions = persistence.SubscriptionSettings();
        subscriptions.CacheFor(TimeSpan.FromMinutes(1));

        #endregion

        #region BehaviorConfig

        endpointConfiguration.Pipeline.Register(typeof(SqlConnectionBehavior),
            "Forwards the NServiceBus SqlConnection/SqlTransaction to data services injected into message handlers.");

        #endregion

        #region DependencyInjectionConfig

        endpointConfiguration.RegisterComponents(register =>
        {
            register.ConfigureComponent<ConnectionHolder>(builder =>
            {
                var ch = new ConnectionHolder();
                return ch;
            }, DependencyLifecycle.InstancePerUnitOfWork);

            register.ConfigureComponent<IDataService>(builder =>
            {
                var connectionHolder = builder.Build<ConnectionHolder>();
                return new DataService(connectionHolder);
            }, DependencyLifecycle.InstancePerUnitOfWork);
        });

        #endregion

        SqlHelper.EnsureDatabaseExists(connectionString);

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        Console.WriteLine("Press S to send a message, or Enter to exit");

        await RunLoop(endpointInstance);

        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }

    static async Task RunLoop(IMessageSession messageSession)
    {
        while (true)
        {
            var key = Console.ReadKey();
            switch (key.Key)
            {
                case ConsoleKey.Enter:
                    return;

                case ConsoleKey.S:
                    await messageSession.SendLocal(new TestMessage { Id = Guid.NewGuid() });
                    break;
            }
        }
    }
}