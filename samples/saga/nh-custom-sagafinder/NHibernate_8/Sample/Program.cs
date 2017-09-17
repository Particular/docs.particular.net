using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Persistence;

class Program
{
    static async Task Main()
    {
        var endpointName = "Samples.NHibernateCustomSagaFinder";
        Console.Title = endpointName;
        var endpointConfiguration = new EndpointConfiguration(endpointName);
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.UseTransport<LearningTransport>();

        #region config

        var persistence = endpointConfiguration.UsePersistence<NHibernatePersistence>();
        var connection = @"Data Source=.\SqlExpress;Database=NsbSamplesNhCustomSagaFinder;Integrated Security=True";
        persistence.ConnectionString(connection);

        #endregion

        SqlHelper.EnsureDatabaseExists(connection);
        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        var startOrder = new StartOrder
        {
            OrderId = "123"
        };
        await endpointInstance.SendLocal(startOrder)
            .ConfigureAwait(false);

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}