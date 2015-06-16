using System;
using NServiceBus;
using Raven.Client;
using Raven.Client.Document;
using StructureMap;

static class Program
{
    static void Main()
    {
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.UoWWithChildContainers");

        #region ContainerConfiguration
        Container container = new Container(x =>
        {
            x.For<IDocumentStore>().Use(new DocumentStore
                        {
                            Url = "http://localhost:32076",
                            DefaultDatabase = "Samples.UoWWithChildContainers"
                        }
                        .Initialize());
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
            Console.Out.WriteLine("Press any key to send a message. Press `q` to quit");

            Console.Out.WriteLine("After storing a few orders you can open a browser and view them at http://localhost:32076/studio/index.html#databases/documents?collection=Orders&database=Samples.UoWWithChildContainers");

            while (Console.ReadKey().ToString() != "q")
            {
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
