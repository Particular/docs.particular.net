using System;
using NServiceBus;
using NServiceBus.MongoDB;

class Program
{

    static void Main()
    {
        #region mongoDbConfig

        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.MongoDB.Server");
        busConfiguration.UsePersistence<MongoDBPersistence>()
            .SetDatabaseName("SamplesMongoDBServer")
            .SetConnectionString("mongodb://localhost:27017");

        #endregion
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.EnableInstallers();

        using (IBus bus = Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}