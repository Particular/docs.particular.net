using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;

static class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.NServiceBus.Extensions.DependencyInjection";

        #region ContainerConfiguration

        var endpointConfiguration = new EndpointConfiguration("Sample");

        // Use the default ServiceProvider factory, using Microsoft's built-in DI container:
        var containerSettings = endpointConfiguration.UseContainer(new DefaultServiceProviderFactory());
        containerSettings.ServiceCollection.AddSingleton(new MyService());

        #endregion

        endpointConfiguration.UseTransport<LearningTransport>();

        var endpoint = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        var myMessage = new MyMessage();
        await endpoint.SendLocal(myMessage)
            .ConfigureAwait(false);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpoint.Stop()
            .ConfigureAwait(false);
    }
}
