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
        using (ReceiverDataContext ctx = new ReceiverDataContext())
        {
            ctx.Database.Initialize(true);
        }

        Configuration hibernateConfig = new Configuration();
        hibernateConfig.DataBaseIntegration(x =>
        {
            x.ConnectionStringName = "NServiceBus/Persistence";
            x.Dialect<MsSql2012Dialect>();
        });

        hibernateConfig.SetProperty("default_schema", "receiver");

        EndpointConfiguration endpointConfiguration = new EndpointConfiguration("Samples.SQLNHibernateOutboxEF.Receiver");
        endpointConfiguration.UseSerialization<JsonSerializer>();

        #region ReceiverConfiguration

        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
        transport.DefaultSchema("receiver");
        transport.UseSpecificSchema(queueName =>
        {
            if (queueName.Equals("error", StringComparison.OrdinalIgnoreCase) || queueName.Equals("audit", StringComparison.OrdinalIgnoreCase))
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

        IEndpointInstance endpoint = await Endpoint.Start(endpointConfiguration);

        try
        {
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
        finally
        {
            await endpoint.Stop();
        }
    }
}
