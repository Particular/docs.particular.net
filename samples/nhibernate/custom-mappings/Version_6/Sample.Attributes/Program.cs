using System;
using System.IO;
using System.Threading.Tasks;
using NHibernate.Cfg;
using NHibernate.Mapping.Attributes;
using NServiceBus;
using NServiceBus.Persistence;
using Environment = NHibernate.Cfg.Environment;

class Program
{

    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Samples.CustomNhMappings.Attributes";
        Configuration nhConfiguration = new Configuration();

        nhConfiguration.SetProperty(Environment.ConnectionProvider, "NHibernate.Connection.DriverConnectionProvider");
        nhConfiguration.SetProperty(Environment.ConnectionDriver, "NHibernate.Driver.Sql2008ClientDriver");
        nhConfiguration.SetProperty(Environment.Dialect, "NHibernate.Dialect.MsSql2008Dialect");
        nhConfiguration.SetProperty(Environment.ConnectionStringName, "NServiceBus/Persistence");

        AddAttributeMappings(nhConfiguration);

        EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
        endpointConfiguration.EndpointName("Samples.CustomNhMappings.Attributes");
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.SendFailedMessagesTo("error");

        endpointConfiguration
            .UsePersistence<NHibernatePersistence>()
            .UseConfiguration(nhConfiguration);

        IEndpointInstance endpoint = await Endpoint.Start(endpointConfiguration);
        try
        {
            await endpoint.SendLocal(new StartOrder
            {
                OrderId = "123"
            });
            await Task.Delay(2000);
            await endpoint.SendLocal(new CompleteOrder
            {
                OrderId = "123"
            });

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
        finally
        {
            await endpoint.Stop();
        }
    }

    #region AttributesConfiguration

    static void AddAttributeMappings(Configuration nhConfiguration)
    {
        HbmSerializer hbmSerializer = new HbmSerializer
        {
            Validate = true
        };

        using (MemoryStream stream = hbmSerializer.Serialize(typeof(Program).Assembly))
        {
            nhConfiguration.AddInputStream(stream);
        }
    }

    #endregion
}