using System;
using NServiceBus;
using Raven.Client;
using StructureMap;

static class Program
{
    static void Main()
    {
        using (var ravenServer = new RavenServer())
        {
            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.EndpointName("Samples.UoWWithChildContainers");

            #region ContainerConfiguration

            Container container = new Container(x =>
            {
                x.For<IDocumentStore>().Use(ravenServer.DocumentStore);
                x.For<IDocumentSession>().Use(c => c.GetInstance<IDocumentStore>().OpenSession());
            });

            busConfiguration.UseContainer<StructureMapBuilder>(c => c.ExistingContainer(container));

            #endregion

            #region PipelineRegistration

            busConfiguration.Pipeline.Register<RavenUnitOfWork.Registration>();

            #endregion

            busConfiguration.UsePersistence<InMemoryPersistence>();
            busConfiguration.EnableInstallers();

            int orderNumber = 1;

            using (IBus bus = Bus.Create(busConfiguration).Start())
            {
                Console.WriteLine("Press enter to send a message");
                Console.WriteLine("Press any key to exit");
                Console.WriteLine("After storing a few orders you can open a browser and view them at " + ravenServer.ManagementUrl);

                while (true)
                {
                    ConsoleKeyInfo key = Console.ReadKey();
                    Console.WriteLine();

                    if (key.Key != ConsoleKey.Enter)
                    {
                        return;
                    }
                    bus.SendLocal(new PlaceOrder
                    {
                        OrderNumber = string.Format("Order-{0}", orderNumber),
                        OrderValue = 100
                    });

                    orderNumber++;
                }
            }
        }
    }
}
