using System;
using System.Threading.Tasks;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NServiceBus;
using NServiceBus.Transport.SQLServer;

class Program
{
    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Samples.EntityFrameworkUnitOfWork.Receiver";
        using (var receiverDataContext = new ReceiverDataContext())
        {
            receiverDataContext.Database.Initialize(true);
        }

        var hibernateConfig = new Configuration();
        hibernateConfig.DataBaseIntegration(x =>
        {
            x.ConnectionStringName = "NServiceBus/Persistence";
            x.Dialect<MsSql2012Dialect>();
        });

        hibernateConfig.SetProperty("default_schema", "receiver");

        var endpointConfiguration = new EndpointConfiguration("Samples.EntityFrameworkUnitOfWork.Receiver");
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.SendFailedMessagesTo("error");

        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();

        var routing = transport.Routing();
        routing.RouteToEndpoint(typeof(OrderAccepted).Assembly, "Samples.EntityFrameworkUnitOfWork.Sender");
        routing.RegisterPublisher(typeof(OrderAccepted).Assembly, "Samples.EntityFrameworkUnitOfWork.Sender");

        transport.DefaultSchema("receiver");

        transport.UseSchemaForEndpoint("Samples.EntityFrameworkUnitOfWork.Sender", "sender");
        transport.UseSchemaForQueue("error", "dbo");
        transport.UseSchemaForQueue("audit", "dbo");

        endpointConfiguration.UsePersistence<NHibernatePersistence>();

        #region ReceiverConfiguration

        var pipeline = endpointConfiguration.Pipeline;
        pipeline.Register(new UnitOfWorkSetupBehaviorBehavior(), "Sets up unit of work for the message");

        #endregion

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