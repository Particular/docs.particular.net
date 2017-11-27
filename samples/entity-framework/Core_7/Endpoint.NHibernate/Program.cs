using System;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Threading.Tasks;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NServiceBus;
using NServiceBus.Persistence;
using NServiceBus.Transport.SQLServer;

class Program
{
    static async Task Main()
    {
        var connection = @"Data Source=.\SqlExpress;Database=NsbSamplesEfUowNh;Integrated Security=True;Max Pool Size=100";
        Console.Title = "Samples.EntityFrameworkUnitOfWork.NHibernate";
        using (var receiverDataContext = new ReceiverDataContext(new SqlConnection(connection)))
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<ReceiverDataContext>());
            receiverDataContext.Database.Initialize(true);
        }

        var endpointConfiguration = new EndpointConfiguration("Samples.EntityFrameworkUnitOfWork.NHibernate");
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.ExecuteTheseHandlersFirst(typeof(CreateOrderHandler), typeof(OrderLifecycleSaga), typeof(CreateShipmentHandler));

        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
        transport.ConnectionString(connection);
        transport.UseNativeDelayedDelivery().DisableTimeoutManagerCompatibility();

        var persistence = endpointConfiguration.UsePersistence<NHibernatePersistence>();
        var hibernateConfig = new Configuration();
        hibernateConfig.DataBaseIntegration(x =>
        {
            x.ConnectionString = connection;
            x.Dialect<MsSql2012Dialect>();
        });

        persistence.UseConfiguration(hibernateConfig);

        #region UnitOfWork_NHibernate

        var pipeline = endpointConfiguration.Pipeline;
        pipeline.Register(new UnitOfWorkSetupBehaviorBehavior(storageSession =>
        {
            var dbConnection = storageSession.Session().Connection;
            var context = new ReceiverDataContext(dbConnection);

            //Don't use transaction because connection is enlisted in the TransactionScope
            context.Database.UseTransaction(null);

            //Call SaveChanges before completing storage session
            storageSession.OnSaveChanges(x => context.SaveChangesAsync());

            return context;
        }), "Sets up unit of work for the message");

        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        await Sender.Start(endpointInstance);
    }
}