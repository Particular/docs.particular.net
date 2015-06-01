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
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.RavenDBCustomSagaFinder");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.EnableInstallers();

        #region RavenDBSetup

        IDocumentStore defaultStore = new DocumentStore
        {
                                          Url = "http://localhost:32076",
                                          DefaultDatabase = "Samples.RavenDBCustomSagaFinder"
                                      }
            .RegisterListener(new UniqueConstraintsStoreListener())
            .Initialize();

        busConfiguration.UsePersistence<RavenDBPersistence>()
            .DoNotSetupDatabasePermissions() //Only required to simplify the sample setup
            .SetDefaultDocumentStore(defaultStore);

        #endregion

        using (IBus bus = Bus.Create(busConfiguration).Start())
        {
            bus.SendLocal(new StartOrder
                          {
                              OrderId = "123"
                          });

            Console.WriteLine("\r\nPress any key to stop program\r\n");
            Console.ReadKey();
        }
    }
}
