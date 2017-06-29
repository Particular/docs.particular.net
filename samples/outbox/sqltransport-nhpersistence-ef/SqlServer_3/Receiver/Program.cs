using System;
using System.Threading.Tasks;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NServiceBus;
using NServiceBus.Transport.SQLServer;
using Configuration = NHibernate.Cfg.Configuration;

class Program
{
    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Samples.SQLNHibernateOutboxEF.Receiver";

        var connectionString = @"Data Source=.\SqlExpress;Database=nservicebus;Integrated Security=True";
        using (var receiverDataContext = new ReceiverDataContext(connectionString))
        {
            await receiverDataContext.Database.EnsureCreatedAsync().ConfigureAwait(false);
        }

        var hibernateConfig = new Configuration();
        hibernateConfig.DataBaseIntegration(x =>
        {
            x.ConnectionStringName = "NServiceBus/Persistence";
            x.Dialect<MsSql2012Dialect>();
        });

        hibernateConfig.SetProperty("default_schema", "receiver");

        var endpointConfiguration = new EndpointConfiguration("Samples.SQLNHibernateOutboxEF.Receiver");
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.SendFailedMessagesTo("error");

        #region ReceiverConfiguration

        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();

        var routing = transport.Routing();
        routing.RouteToEndpoint(typeof(OrderAccepted).Assembly, "Samples.SQLNHibernateOutboxEF.Sender");
        routing.RegisterPublisher(typeof(OrderAccepted).Assembly, "Samples.SQLNHibernateOutboxEF.Sender");

        transport.DefaultSchema("receiver");

        transport.UseSchemaForEndpoint("Samples.SQLNHibernateOutboxEF.Sender", "sender");
        transport.UseSchemaForQueue("error", "dbo");
        transport.UseSchemaForQueue("audit", "dbo");

        endpointConfiguration
            .UsePersistence<NHibernatePersistence>();

        endpointConfiguration.EnableOutbox();

        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}