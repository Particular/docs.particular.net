using System;
using System.Threading.Tasks;
using NServiceBus;
using StructureMap;

static class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.StructureMap";

        #region ContainerConfiguration

        var endpointConfiguration = new EndpointConfiguration("Samples.StructureMap");

        var registry = new Registry();
        registry.For<MyService>().Use(new MyService());

        endpointConfiguration.UseContainer(new StructureMapServiceProviderFactory(registry));

        #endregion

        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseTransport<LearningTransport>();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        var myMessage = new MyMessage();
        await endpointInstance.SendLocal(myMessage)
            .ConfigureAwait(false);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}