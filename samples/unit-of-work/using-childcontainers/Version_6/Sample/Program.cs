using System;
using System.Threading.Tasks;
using NServiceBus;
using Raven.Client;
using Raven.Client.Document;
using StructureMap;

static class Program
{
    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        using (new RavenHost())
        {
            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.EndpointName("Samples.UoWWithChildContainers");
            busConfiguration.SendFailedMessagesTo("error");

            #region ContainerConfiguration

            DocumentStore documentStore = new DocumentStore
            {
                Url = "http://localhost:32076",
                DefaultDatabase = "NServiceBus"
            };
            documentStore.Initialize();

            Container container = new Container(x =>
            {
                x.For<IDocumentStore>().Use(documentStore);
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

            IEndpointInstance endpoint = await Endpoint.Start(busConfiguration);
            try
            {
                IBusSession busSession = endpoint.CreateBusSession();
                Console.WriteLine("Press enter to send a message");
                Console.WriteLine("Press any key to exit");
                Console.WriteLine("After storing a few orders you can open a browser and view them at http://localhost:32076");

                while (true)
                {
                    ConsoleKeyInfo key = Console.ReadKey();
                    Console.WriteLine();

                    if (key.Key != ConsoleKey.Enter)
                    {
                        return;
                    }
                    await busSession.SendLocal(new PlaceOrder
                    {
                        OrderNumber = $"Order-{orderNumber}",
                        OrderValue = 100
                    });

                    orderNumber++;
                }
            }
            finally
            {
                await endpoint.Stop();
            }
        }
    }
}
