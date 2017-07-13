using System;
using NServiceBus;
using NServiceBus.Persistence.MongoDB;

class Program
{

    static void Main()
    {
        Console.Title = "Samples.MongoDB.Server";
        #region mongoDbConfig

        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.MongoDB.Server");
        var persistence = busConfiguration.UsePersistence<MongoDbPersistence>();
        persistence.SetConnectionString("mongodb://localhost:27017/SamplesMongoDBServer");

        #endregion
        busConfiguration.EnableInstallers();

        using (var bus = Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}