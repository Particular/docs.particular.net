using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;

static class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.NServiceBus.Extensions.DependencyInjection";

        var endpointConfiguration = new EndpointConfiguration("Sample");
        endpointConfiguration.UseTransport<LearningTransport>();

        #region ContainerConfiguration

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton<MyService>();
        serviceCollection.AddSingleton<MessageSenderService>();

        var endpointWithExternallyManagedServiceProvider = EndpointWithExternallyManagedServiceProvider
            .Create(endpointConfiguration, serviceCollection);
        // if needed register the session
        serviceCollection.AddSingleton(p => endpointWithExternallyManagedServiceProvider.MessageSession.Value);

        #endregion

        using (var serviceProvider = serviceCollection.BuildServiceProvider())
        {
            var endpoint = await endpointWithExternallyManagedServiceProvider.Start(serviceProvider)
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
