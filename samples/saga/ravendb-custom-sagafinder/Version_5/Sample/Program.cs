using System;
using NServiceBus;
using NServiceBus.Persistence;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.UniqueConstraints;

class Program
{

    static void Main()
    {
        BusConfiguration busConfig = new BusConfiguration();
        busConfig.EndpointName("Samples.RavenDBCustomSagaFinder");
        busConfig.UseSerialization<JsonSerializer>();
        busConfig.EnableInstallers();

        #region RavenDBSetup

        IDocumentStore defaultStore = new DocumentStore()
                                      {
                                          Url = "http://localhost:8080",
                                          DefaultDatabase = "Samples.RavenDBCustomSagaFinder"
                                      }
            .RegisterListener(new UniqueConstraintsStoreListener())
            .Initialize();

        busConfig.UsePersistence<RavenDBPersistence>()
            .DoNotSetupDatabasePermissions() //Only required to simplify the sample setup
            .SetDefaultDocumentStore(defaultStore);

        #endregion

        using (IStartableBus bus = Bus.Create(busConfig))
        {
            bus.Start();
            bus.SendLocal(new StartOrder
                          {
                              OrderId = "123"
                          });

            Console.WriteLine("\r\nPress any key to stop program\r\n");
            Console.ReadKey();
        }
    }
}
