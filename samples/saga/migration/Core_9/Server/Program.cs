using System;
using System.Threading.Tasks;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;
using NServiceBus;
using NServiceBus.Persistence;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.SagaMigration.Server";

        var endpointConfiguration = new EndpointConfiguration("Samples.SagaMigration.Server");
        endpointConfiguration.UseTransport(new LearningTransport());
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.UseSerialization<XmlSerializer>();
        var persistence = endpointConfiguration.UsePersistence<NHibernatePersistence>();

        // for SqlExpress use Data Source=.\SqlExpress;Initial Catalog=NsbSamplesSagaMigration;Integrated Security=True;Max Pool Size=100;Encrypt=false
        var connectionString = @"Server=localhost,1433;Initial Catalog=NsbSamplesSagaMigration;User Id=SA;Password=yourStrong(!)Password;Max Pool Size=100;Encrypt=false";
        var hibernateConfig = new Configuration();
        hibernateConfig.DataBaseIntegration(x =>
        {
            x.ConnectionString = connectionString;
            x.Dialect<MsSql2012Dialect>();
            x.Driver<MicrosoftDataSqlClientDriver>();
        });

        persistence.UseConfiguration(hibernateConfig);

        SqlHelper.EnsureDatabaseExists(connectionString);
        var endpoint = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        Console.WriteLine("Press <enter> to exit.");
        Console.ReadLine();

        await endpoint.Stop()
            .ConfigureAwait(false);
    }
}