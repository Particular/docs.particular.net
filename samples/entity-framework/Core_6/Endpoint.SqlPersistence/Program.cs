using System;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Persistence.Sql;
using NServiceBus.Transport.SQLServer;

class Program
{
    static async Task Main()
    {
        var connection = @"Data Source=.\SqlExpress;Database=NsbSamplesEfUowSql;Integrated Security=True;Max Pool Size=100";
        Console.Title = "Samples.EntityFrameworkUnitOfWork.SQL";
        using (var receiverDataContext = new ReceiverDataContext(new SqlConnection(connection)))
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<ReceiverDataContext>());
            receiverDataContext.Database.Initialize(true);
        }

        var endpointConfiguration = new EndpointConfiguration("Samples.EntityFrameworkUnitOfWork.SQL");
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.Recoverability().DisableLegacyRetriesSatellite();
        endpointConfiguration.ExecuteTheseHandlersFirst(typeof(CreateOrderHandler), typeof(OrderLifecycleSaga), typeof(CreateShipmentHandler));

        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
        transport.ConnectionString(connection);
        transport.UseNativeDelayedDelivery().DisableTimeoutManagerCompatibility();

        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        persistence.ConnectionBuilder(() => new SqlConnection(connection));
        persistence.SubscriptionSettings().DisableCache();
        var dialect = persistence.SqlDialect<SqlDialect.MsSqlServer>();

        #region UnitOfWork_SQL

        var pipeline = endpointConfiguration.Pipeline;
        pipeline.Register(new UnitOfWorkSetupBehaviorBehavior(storageSession =>
        {
            var dbConnection = storageSession.SqlPersistenceSession().Connection;
            var context = new ReceiverDataContext(dbConnection);

            //Use the same underlying ADO.NET transaction
            context.Database.UseTransaction(storageSession.SqlPersistenceSession().Transaction);

            //Call SaveChanges before completing storage session
            storageSession.SqlPersistenceSession().OnSaveChanges(x => context.SaveChangesAsync());

            return context;
        }), "Sets up unit of work for the message");

        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        await Sender.Start(endpointInstance);
    }
}