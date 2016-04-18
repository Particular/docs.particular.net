using System;
using System.Threading.Tasks;
using NServiceBus;
using StructureMap;

static class Program
{
    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Samples.StructureMap";
        #region ContainerConfiguration
        EndpointConfiguration endpointConfiguration = new EndpointConfiguration("Samples.StructureMap");
        Container container = new Container(x => x.For<MyService>().Use(new MyService()));
        endpointConfiguration.UseContainer<StructureMapBuilder>(c => c.ExistingContainer(container));
        #endregion
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.SendFailedMessagesTo("error");

        IEndpointInstance endpoint = await Endpoint.Start(endpointConfiguration);
        try
        {
            await endpoint.SendLocal(new MyMessage());
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();

        }
        finally
        {
            await endpoint.Stop();
        }
    }
}