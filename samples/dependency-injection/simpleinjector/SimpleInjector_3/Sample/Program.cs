using System;
using System.Threading.Tasks;
using NServiceBus;

static class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.SimpleInjector";

        #region ContainerConfiguration

        var endpointConfiguration = new EndpointConfiguration("Samples.SimpleInjector");

        endpointConfiguration.RegisterComponents(
            registration: components =>
            {
                components.RegisterSingleton(new MyService());
            });
        endpointConfiguration.UseContainer<SimpleInjectorBuilder>();

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