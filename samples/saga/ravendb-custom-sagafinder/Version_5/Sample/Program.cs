using System;
using NServiceBus;
using NServiceBus.Persistence;
using Raven.Client.Embedded;
using Raven.Client.UniqueConstraints;

class Program
{

    static void Main()
    {
        using (var documentStore = new EmbeddableDocumentStore
            {
                DataDirectory = "Data",
                Configuration =
                {
                    PluginsDirectory = Environment.CurrentDirectory,
                }
            })
        {
            #region RavenDBSetup

            documentStore
                .RegisterListener(new UniqueConstraintsStoreListener())
                .Initialize();

            #endregion

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.EndpointName("Samples.RavenDBCustomSagaFinder");
            busConfiguration.UseSerialization<JsonSerializer>();
            busConfiguration.EnableInstallers();

            busConfiguration.UsePersistence<RavenDBPersistence>()
                .DoNotSetupDatabasePermissions() //Only required to simplify the sample setup
                .SetDefaultDocumentStore(documentStore);


            using (IBus bus = Bus.Create(busConfiguration).Start())
            {
                bus.SendLocal(new StartOrder
                {
                    OrderId = "123"
                });

                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
            }
        }
    }
}