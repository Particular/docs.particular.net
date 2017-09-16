using System;
using System.Threading.Tasks;
using NServiceBus;
using Spring.Context.Support;

static class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.Spring";

        #region ContainerConfiguration

        var endpointConfiguration = new EndpointConfiguration("Samples.Spring");
        var applicationContext = new GenericApplicationContext();
        applicationContext.ObjectFactory
            .RegisterSingleton("MyService", new MyService());
        endpointConfiguration.UseContainer<SpringBuilder>(
            customizations: customizations =>
            {
                customizations.ExistingApplicationContext(applicationContext);
            });

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