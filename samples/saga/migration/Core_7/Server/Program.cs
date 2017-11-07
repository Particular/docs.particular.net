using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Persistence;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.SagaMigration.Server";

        var endpointConfiguration = new EndpointConfiguration("Samples.SagaMigration.Server");
        endpointConfiguration.UseTransport<LearningTransport>();
        endpointConfiguration.EnableInstallers();
        var persistence = endpointConfiguration.UsePersistence<NHibernatePersistence>();
        var connection = @"Data Source=.\SqlExpress;Initial Catalog=NsbSamplesSagaMigration;Integrated Security=True;";
        persistence.ConnectionString(connection);

        SqlHelper.EnsureDatabaseExists(connection);
        var endpoint = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        Console.WriteLine("Press <enter> to exit.");
        Console.ReadLine();

        await endpoint.Stop()
            .ConfigureAwait(false);
    }
}