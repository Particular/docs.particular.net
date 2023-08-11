using System;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NServiceBus;
using NServiceBus.Faults;
using NServiceBus.Persistence.Sql;
using NServiceBus.Transport.SqlServer;
using System.Transactions;

class Program
{
    static async Task Main()
    {
        // for SqlExpress use Data Source=.\SqlExpress;Initial Catalog=NsbSamplesEfCoreUowSql;Integrated Security=True;Encrypt=false
        var connectionString = @"Server=localhost,1433;Initial Catalog=NsbSamplesEfCoreUowSql;User Id=SA;Password=yourStrong(!)Password;Encrypt=false";
        Console.Title = "Samples.EntityFrameworkUnitOfWork.SQL";

        using (var receiverDataContext = new ReceiverDataContext(new DbContextOptionsBuilder<ReceiverDataContext>()
            .UseSqlServer(new SqlConnection(connectionString))
            .Options))
        {
            await receiverDataContext.Database.EnsureCreatedAsync().ConfigureAwait(false);
        }        

        var endpointConfiguration = new EndpointConfiguration("Samples.EntityFrameworkUnitOfWork.SQL");
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.ExecuteTheseHandlersFirst(typeof(CreateOrderHandler), typeof(OrderLifecycleSaga), typeof(CreateShipmentHandler));

        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
        transport.ConnectionString(connectionString);        
        transport.SubscriptionSettings().DisableSubscriptionCache();
        transport.Transactions(TransportTransactionMode.SendsAtomicWithReceive);

        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        persistence.ConnectionBuilder(() => new SqlConnection(connectionString));
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
