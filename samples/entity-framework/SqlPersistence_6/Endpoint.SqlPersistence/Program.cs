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
        var connectionString = @"Data Source=.\sqlexpress;Database=NsbSamplesEfUowSql;Integrated Security=True;Max Pool Size=100";
        Console.Title = "Samples.EntityFrameworkUnitOfWork.SQL";
        using (var connection = new SqlConnection(connectionString))
        {
            using (var receiverDataContext = new ReceiverDataContext(connection))
            {
                Database.SetInitializer(new CreateDatabaseIfNotExists<ReceiverDataContext>());
                receiverDataContext.Database.Initialize(true);
            }
        }

        var endpointConfiguration = new EndpointConfiguration("Samples.EntityFrameworkUnitOfWork.SQL");
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.ExecuteTheseHandlersFirst(typeof(CreateOrderHandler), typeof(OrderLifecycleSaga), typeof(CreateShipmentHandler));

        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
        transport.ConnectionString(connectionString);
        transport.Transactions(TransportTransactionMode.SendsAtomicWithReceive);
        transport.NativeDelayedDelivery().DisableTimeoutManagerCompatibility();
        transport.SubscriptionSettings().DisableSubscriptionCache();

        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        persistence.ConnectionBuilder(() => new SqlConnection(connectionString));
        var dialect = persistence.SqlDialect<SqlDialect.MsSqlServer>();

        #region UnitOfWork_SQL

        endpointConfiguration.RegisterComponents(c =>
        {
            c.ConfigureComponent(b =>
            {
                var session = b.Build<ISqlStorageSession>();
                var context = new ReceiverDataContext(session.Connection);

                //Use the same underlying ADO.NET transaction
                context.Database.UseTransaction(session.Transaction);

                //Ensure context is flushed before the transaction is committed
                session.OnSaveChanges(s => context.SaveChangesAsync());

                return context;
            }, DependencyLifecycle.InstancePerUnitOfWork);
        });

        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        await Sender.Start(endpointInstance);
    }
}