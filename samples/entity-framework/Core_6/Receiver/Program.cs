using System;
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
        var connection = @"Data Source=.\SqlExpress;Database=NsbSamplesEfUow;Integrated Security=True";
        Console.Title = "Samples.EntityFrameworkUnitOfWork.Receiver";
        using (var receiverDataContext = new ReceiverDataContext(new SqlConnection(connection)))
        {
            receiverDataContext.Database.Initialize(true);
        }



        var endpointConfiguration = new EndpointConfiguration("Samples.EntityFrameworkUnitOfWork.Receiver");
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.SendFailedMessagesTo("error");

        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
        transport.ConnectionString(connection);
        var routing = transport.Routing();
        routing.RouteToEndpoint(typeof(OrderAccepted).Assembly, "Samples.EntityFrameworkUnitOfWork.Sender");
        routing.RegisterPublisher(typeof(OrderAccepted).Assembly, "Samples.EntityFrameworkUnitOfWork.Sender");

        transport.DefaultSchema("receiver");

        transport.UseSchemaForEndpoint("Samples.EntityFrameworkUnitOfWork.Sender", "sender");
        transport.UseSchemaForQueue("error", "dbo");
        transport.UseSchemaForQueue("audit", "dbo");

        var persistence = endpointConfiguration.UsePersistence<NHibernatePersistence>();
        var hibernateConfig = new Configuration();
        hibernateConfig.DataBaseIntegration(x =>
        {
            x.ConnectionString = connection;
            x.Dialect<MsSql2012Dialect>();
        });

        hibernateConfig.SetProperty("default_schema", "receiver");
        persistence.UseConfiguration(hibernateConfig);

        #region ReceiverConfiguration

        var pipeline = endpointConfiguration.Pipeline;
        pipeline.Register(new UnitOfWorkSetupBehaviorBehavior(), "Sets up unit of work for the message");

        #endregion

        SqlHelper.CreateSchema(connection, "receiver");
        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        try
        {
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
        finally
        {
            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }
    }
}