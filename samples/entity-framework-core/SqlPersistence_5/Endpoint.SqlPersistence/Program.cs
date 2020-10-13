using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NServiceBus;
using NServiceBus.Persistence.Sql;
using NServiceBus.Transport.SQLServer;

class Program
{
    static async Task Main()
    {
        var connection = @"Data Source=(local)\SqlExpress;Database=nservicebus;Integrated Security=True;Max Pool Size=100";
        Console.Title = "Samples.EntityFrameworkUnitOfWork.SQL";

        using (var receiverDataContext = new ReceiverDataContext(new DbContextOptionsBuilder<ReceiverDataContext>()
            .UseSqlServer(new SqlConnection(connection))
            .Options))
        {
            await receiverDataContext.Database.EnsureCreatedAsync().ConfigureAwait(false);
        }

        var endpointConfiguration = new EndpointConfiguration("Samples.EntityFrameworkUnitOfWork.SQL");
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.ExecuteTheseHandlersFirst(typeof(CreateOrderHandler), typeof(OrderLifecycleSaga), typeof(CreateShipmentHandler));

        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
        transport.ConnectionString(connection);
        transport.NativeDelayedDelivery().DisableTimeoutManagerCompatibility();
        transport.SubscriptionSettings().DisableSubscriptionCache();

        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        persistence.ConnectionBuilder(() => new SqlConnection(connection));
        var dialect = persistence.SqlDialect<SqlDialect.MsSqlServer>();

        #region UnitOfWork_SQL

        var pipeline = endpointConfiguration.Pipeline;
        pipeline.Register(new UnitOfWorkSetupBehavior(storageSession =>
        {
            var dbConnection = storageSession.SqlPersistenceSession().Connection;

            var context = new ReceiverDataContext(new DbContextOptionsBuilder<ReceiverDataContext>()
                .UseSqlServer(dbConnection)
                .Options);

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