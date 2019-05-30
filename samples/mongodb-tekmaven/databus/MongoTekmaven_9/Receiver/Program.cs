using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Persistence.MongoDB;
using NServiceBus.Persistence.MongoDB.DataBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.DataBus.Receiver";
        var endpointConfiguration = new EndpointConfiguration("Samples.DataBus.Receiver");
        var persistence = endpointConfiguration.UsePersistence<MongoDbPersistence>();
        persistence.SetConnectionString("mongodb://localhost:27017/SamplesMongoDBServer");
        var dataBus = endpointConfiguration.UseDataBus<MongoDbDataBus>();
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