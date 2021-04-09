using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;

static class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.NServiceBus.ExternallyManagedContainer";

        var endpointConfiguration = new EndpointConfiguration("Sample");
        endpointConfiguration.UseTransport(new LearningTransport());

        #region ContainerConfiguration

        // ServiceCollection is provided by Microsoft.Extensions.DependencyInjection
        var serviceCollection = new ServiceCollection();

        // most dependencies may now be registered
        serviceCollection.AddSingleton<Greeter>();
        serviceCollection.AddSingleton<MessageSender>();

        // EndpointWithExternallyManagedContainer.Create accepts an IServiceCollection,
        // which is inherited by ServiceCollection
        var endpointWithExternallyManagedContainer = EndpointWithExternallyManagedContainer
            .Create(endpointConfiguration, serviceCollection);

        // if IMessageSession is required as dependency, it may now be registered
        serviceCollection.AddSingleton(p => endpointWithExternallyManagedContainer.MessageSession.Value);

        #endregion

        using (var serviceProvider = serviceCollection.BuildServiceProvider())
        {
            var endpoint = await endpointWithExternallyManagedContainer.Start(serviceProvider)
                .ConfigureAwait(false);

            var sender = serviceProvider.GetRequiredService<MessageSender>();
            await sender.SendMessage()
                .ConfigureAwait(false);

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
            await endpoint.Stop()
                .ConfigureAwait(false);
        }
    }
}
