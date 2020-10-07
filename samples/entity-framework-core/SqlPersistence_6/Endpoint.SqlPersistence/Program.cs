using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NServiceBus;
using NServiceBus.Faults;
using NServiceBus.Persistence.Sql;
using NServiceBus.Transport.SQLServer;

class Program
{
    static async Task Main()
    {
        var connection = @"server=(local)\sqlexpress;database=nservicebus;Integrated Security=True;Max Pool Size=100";
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

        endpointConfiguration.RegisterComponents(c =>
        {
            c.ConfigureComponent(b =>
            {
                var session = b.Build<ISqlStorageSession>();

                var context = new ReceiverDataContext(new DbContextOptionsBuilder<ReceiverDataContext>()
                    .UseSqlServer(session.Connection)
                    .Options);

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