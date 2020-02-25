using System;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Castle.Windsor.MsDependencyInjection;
using NServiceBus;

static class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.Castle";

        #region ContainerConfiguration

        var endpointConfiguration = new EndpointConfiguration("Samples.Castle");

        var containerSettings = endpointConfiguration.UseContainer(new WindsorServiceProviderFactory());

        containerSettings.ConfigureContainer(c => c.Register(Component.For<MyService>()
            .Instance(new MyService())));

        #endregion

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