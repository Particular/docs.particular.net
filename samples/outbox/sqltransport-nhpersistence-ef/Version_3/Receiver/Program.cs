using System;
using System.Threading.Tasks;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NServiceBus;
using NServiceBus.Transports.SQLServer;
using NServiceBus.Persistence;

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

        EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
        endpointConfiguration.UseSerialization<JsonSerializer>();

        #region ReceiverConfiguration

        endpointConfiguration
            .UseTransport<SqlServerTransport>()
            .DefaultSchema("receiver")
            .UseSpecificSchema(queueName =>
            {
                if (queueName.Equals("error", StringComparison.OrdinalIgnoreCase) || queueName.Equals("audit", StringComparison.OrdinalIgnoreCase))
                {
                    return "dbo";
                }
                if (queueName.Equals("sender", StringComparison.OrdinalIgnoreCase))
                {
                    return "sender";
                }
                return null;
            });

        endpointConfiguration
            .UsePersistence<NHibernatePersistence>()
            .RegisterManagedSessionInTheContainer();

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
