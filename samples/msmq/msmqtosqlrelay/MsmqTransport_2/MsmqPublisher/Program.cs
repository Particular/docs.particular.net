using System;
using System.Threading.Tasks;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;
using NServiceBus;
using NServiceBus.Persistence;
using Shared;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.MsmqToSqlRelay.MsmqPublisher";
        #region publisher-config

        var endpointConfiguration = new EndpointConfiguration("MsmqPublisher");
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        endpointConfiguration.UseTransport(new MsmqTransport());
        endpointConfiguration.EnableInstallers();
        var persistence = endpointConfiguration.UsePersistence<NHibernatePersistence>();
        // for SqlExpress use Data Source=.\SqlExpress;Initial Catalog=PersistenceForMsmqTransport;Integrated Security=True;Max Pool Size=100;Encrypt=false
        var connectionString = @"Server=localhost,1433;Initial Catalog=PersistenceForMsmqTransport;User Id=SA;Password=yourStrong(!)Password;Max Pool Size=100;Encrypt=false";
        var hibernateConfig = new Configuration();
        hibernateConfig.DataBaseIntegration(x =>
        {
            x.ConnectionString = connectionString;
            x.Dialect<MsSql2012Dialect>();
            x.Driver<MicrosoftDataSqlClientDriver>();
        });
        persistence.UseConfiguration(hibernateConfig);
        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration);
        await Start(endpointInstance);
        await endpointInstance.Stop();
    }

    static async Task Start(IMessageSession messageSession)
    {
        Console.WriteLine("Press Enter to publish the SomethingHappened Event");
        Console.WriteLine("Press any key to exit");

        #region publisher-loop
        while (true)
        {
            var key = Console.ReadKey();
            Console.WriteLine();

            if (key.Key != ConsoleKey.Enter)
            {
                return;
            }
            await messageSession.Publish(new SomethingHappened());
            Console.WriteLine("SomethingHappened Event published");
        }
        #endregion
    }
}
