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
        var endpointConfiguration = new EndpointConfiguration("Samples.StructureMap");
        var container = new Container(x => x.For<MyService>().Use(new MyService()));
        endpointConfiguration.UseContainer<StructureMapBuilder>(c => c.ExistingContainer(container));
        #endregion
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.SendFailedMessagesTo("error");

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        try
        {
            var myMessage = new MyMessage();
            await endpointInstance.SendLocal(myMessage)
                .ConfigureAwait(false);
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
        finally
        {
            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }
    }
}