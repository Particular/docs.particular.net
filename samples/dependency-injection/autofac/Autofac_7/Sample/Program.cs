using System;
using System.Threading.Tasks;
using Autofac;
using NServiceBus;

static class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.Autofac";

        #region ContainerConfiguration

        var endpointConfiguration = new EndpointConfiguration("Samples.Autofac");

        var builder = new ContainerBuilder();

        builder.Register(x => Endpoint.Start(endpointConfiguration))
            .As<Task<IEndpointInstance>>()
            .SingleInstance();

        builder.RegisterInstance(new MyService());

        var container = builder.Build();

        endpointConfiguration.UseContainer<AutofacBuilder>(
            customizations: customizations =>
            {
                customizations.ExistingLifetimeScope(container);
            });

        #endregion

        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseTransport<LearningTransport>();

        var endpointInstance = await container.Resolve<Task<IEndpointInstance>>().ConfigureAwait(false);

        var myMessage = new MyMessage();
        await endpointInstance.SendLocal(myMessage)
            .ConfigureAwait(false);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}
