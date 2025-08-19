using System;
using System.Threading.Tasks;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Mapping.Attributes;
using NServiceBus;
using NServiceBus.Persistence;

class Program
{
    static async Task Main()
    {
        Console.Title = "Attributes";

        var endpointConfiguration = new EndpointConfiguration("Samples.CustomNhMappings.Attributes");
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        endpointConfiguration.UseTransport(new LearningTransport());

        // for SqlExpress use Data Source=.\SqlExpress;Initial Catalog=Samples.CustomNhMappings;Integrated Security=True;Max Pool Size=100;Encrypt=false
        var connectionString = @"Server=localhost,1433;Initial Catalog=Samples.CustomNhMappings;User Id=SA;Password=yourStrong(!)Password;Max Pool Size=100;Encrypt=false";
        var hibernateConfig = new Configuration();
        hibernateConfig.DataBaseIntegration(x =>
        {
            x.ConnectionString = connectionString;
            x.Dialect<MsSql2012Dialect>();
            x.Driver<MicrosoftDataSqlClientDriver>();
        });

        AddAttributeMappings(hibernateConfig);

        var persistence = endpointConfiguration.UsePersistence<NHibernatePersistence>();
        persistence.UseConfiguration(hibernateConfig);

        var endpoint = await Endpoint.Start(endpointConfiguration);
        var startOrder = new StartOrder
        {
            OrderId = "123"
        };
        await endpoint.SendLocal(startOrder);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpoint.Stop();
    }

    #region AttributesConfiguration

    static void AddAttributeMappings(Configuration nhConfiguration)
    {
        var hbmSerializer = new HbmSerializer
        {
            Validate = true
        };

        using var stream = hbmSerializer.Serialize(typeof(Program).Assembly);
        nhConfiguration.AddInputStream(stream);
    }

    #endregion
}
