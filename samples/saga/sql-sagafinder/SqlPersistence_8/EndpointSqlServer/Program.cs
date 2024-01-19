using System;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Persistence.Sql;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.SqlSagaFinder.SqlServer";
        var endpointConfiguration = new EndpointConfiguration("Samples.SqlSagaFinder.SqlServer");
        endpointConfiguration.UseTransport(new LearningTransport());
        endpointConfiguration.UseSerialization<XmlSerializer>();
        endpointConfiguration.EnableInstallers();

        #region sqlServerConfig

        // for SqlExpress use Data Source=.\SqlExpress;Initial Catalog=NsbSamplesSqlSagaFinder;Integrated Security=True;Encrypt=false
        var connectionString = @"Server=localhost,1433;Initial Catalog=NsbSamplesSqlSagaFinder;User Id=SA;Password=yourStrong(!)Password;Encrypt=false";
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

        await SqlHelper.EnsureDatabaseExists(connectionString);
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