using System;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using NServiceBus.Persistence.Sql;

class Program
{
    static async Task Main()
    {        
        // for SqlExpress use Data Source=.\SqlExpress;Initial Catalog=NsbSamplesEfUowSql;Integrated Security=True;Encrypt=false
        var connectionString = @"Server=localhost,1433;Initial Catalog=NsbSamplesEfUowSql;User Id=SA;Password=yourStrong(!)Password;Encrypt=false";

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

        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        endpointConfiguration.UseTransport(new SqlServerTransport(connectionString)
        {
            TransportTransactionMode = TransportTransactionMode.SendsAtomicWithReceive,
            Subscriptions =
            {
                DisableCaching = true
            }
        });

        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        persistence.ConnectionBuilder(() => new SqlConnection(connectionString));
        var dialect = persistence.SqlDialect<SqlDialect.MsSqlServer>();

        #region UnitOfWork_SQL

        endpointConfiguration.RegisterComponents(c =>
        {
            c.AddScoped(b =>
            {
                var session = b.GetRequiredService<ISqlStorageSession>();
                var context = new ReceiverDataContext(session.Connection);

                //Use the same underlying ADO.NET transaction
                context.Database.UseTransaction(session.Transaction);

                //Ensure context is flushed before the transaction is committed
                session.OnSaveChanges((s, cancellationToken) => context.SaveChangesAsync(cancellationToken));

                return context;
            });
        });

        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        await Sender.Start(endpointInstance);
    }
}
