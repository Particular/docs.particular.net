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
        Console.Title = "Samples.SQLNHibernateOutboxEF.Receiver";
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

        var endpointConfiguration = new EndpointConfiguration("Samples.SQLNHibernateOutboxEF.Receiver");
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.EnableInstallers();

        #region ReceiverConfiguration

        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
        transport.DefaultSchema("receiver");
        transport.UseSpecificSchema(queueName =>
        {
            if (queueName.Equals("error", StringComparison.OrdinalIgnoreCase) ||
                queueName.Equals("audit", StringComparison.OrdinalIgnoreCase))
            {
                return "dbo";
            }
            if (queueName.Equals("Samples.SQLNHibernateOutboxEF.Sender", StringComparison.OrdinalIgnoreCase))
            {
                return "sender";
            }
            return null;
        });

        endpointConfiguration
            .UsePersistence<NHibernatePersistence>();

        endpointConfiguration.EnableOutbox();

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