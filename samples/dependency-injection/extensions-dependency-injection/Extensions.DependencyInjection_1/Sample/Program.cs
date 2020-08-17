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
        endpointConfiguration.UseTransport<LearningTransport>();

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton<MyService>();
        serviceCollection.AddSingleton<MessageSenderService>();

        var endpointWithExternallyManagedContainer = EndpointWithExternallyManagedServiceProvider.Create(endpointConfiguration, serviceCollection);
        // if needed register the session
        serviceCollection.AddSingleton(p => endpointWithExternallyManagedContainer.MessageSession.Value);

        #endregion

        using (var serviceProvider = serviceCollection.BuildServiceProvider())
        {
            var endpoint = await endpointWithExternallyManagedContainer.Start(serviceProvider)
                .ConfigureAwait(false);

            var senderService = serviceProvider.GetRequiredService<MessageSenderService>();
            await senderService.SendMessage()
                .ConfigureAwait(false);

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
            await endpoint.Stop()
                .ConfigureAwait(false);
        }
    }
}
