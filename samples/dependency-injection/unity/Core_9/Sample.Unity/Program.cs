using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using Unity;
using Unity.Microsoft.DependencyInjection;

static class Program
{
    static async Task Main()
    {
        Console.Title = "Sample.Unity";

        var endpointConfiguration = new EndpointConfiguration("Sample.Unity");
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        endpointConfiguration.UseTransport(new LearningTransport());

        #region ContainerConfiguration

        // ServiceCollection is provided by Microsoft.Extensions.DependencyInjection
        var serviceCollection = new ServiceCollection();
       
     // EndpointWithExternallyManagedContainer.Create accepts an IServiceCollection,
        // which is inherited by ServiceCollection
        var endpointWithExternallyManagedContainer = EndpointWithExternallyManagedContainer
            .Create(endpointConfiguration, serviceCollection);

        // most dependencies may now be registered
        serviceCollection.AddSingleton<Greeter>();
        serviceCollection.AddSingleton<MessageSender>();

        // if IMessageSession is required as dependency, it may now be registered
        serviceCollection.AddSingleton(p => endpointWithExternallyManagedContainer.MessageSession.Value);

        var container = new UnityContainer();
        var serviceProvider = container.BuildServiceProvider(serviceCollection);

        #endregion

        var endpoint = await endpointWithExternallyManagedContainer.Start(serviceProvider);

        var sender = serviceProvider.GetRequiredService<MessageSender>();
        await sender.SendMessage();

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpoint.Stop();
    }
}