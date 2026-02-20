using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Server";

        #region mongoDbConfig

        var endpointConfiguration = new EndpointConfiguration("Samples.MongoDB.Server");
        var persistence = endpointConfiguration.UsePersistence<MongoPersistence>();
        persistence.DatabaseName("Samples_MongoDB_Server");

        #endregion

        endpointConfiguration.EnableInstallers();
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        endpointConfiguration.UseTransport(new LearningTransport());

        var endpointInstance = await Endpoint.Start(endpointConfiguration);

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();

        await endpointInstance.Stop();
    }
}
