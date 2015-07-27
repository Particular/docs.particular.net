using System;
using NServiceBus;
using NServiceBus.Persistence;
using Raven.Client.UniqueConstraints;

class Program
{

    static void Main()
    {
        using (var ravenServer = new RavenServer(x => x.RegisterListener(new UniqueConstraintsStoreListener())))
        {
            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.EndpointName("Samples.RavenDBCustomSagaFinder");
            busConfiguration.UseSerialization<JsonSerializer>();
            busConfiguration.EnableInstallers();

            busConfiguration.UsePersistence<RavenDBPersistence>()
                .DoNotSetupDatabasePermissions() //Only required to simplify the sample setup
                .SetDefaultDocumentStore(ravenServer.DocumentStore);

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

