using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Persistence.MongoDB;

class Program
{

    static async Task Main()
    {
        Console.Title = "Samples.MongoDB.Server";

        #region mongoDbConfig

        var endpointConfiguration = new EndpointConfiguration("Samples.MongoDB.Server");
        var persistence = endpointConfiguration.UsePersistence<MongoDbPersistence>();
        persistence.SetConnectionString("mongodb://localhost:27017/SamplesMongoDBServer");

        #endregion

        endpointConfiguration.EnableInstallers();
        endpointConfiguration.UseTransport<LearningTransport>();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}