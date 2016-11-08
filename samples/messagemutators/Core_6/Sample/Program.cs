using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Samples.MessageMutators";
        var endpointConfiguration = new EndpointConfiguration("Samples.MessageMutators");
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.SendFailedMessagesTo("error");

        #region ComponentRegistration

        endpointConfiguration.RegisterComponents(
            registration: components =>
            {
                components.ConfigureComponent<ValidationMessageMutator>(DependencyLifecycle.InstancePerCall);
                components.ConfigureComponent<TransportMessageCompressionMutator>(DependencyLifecycle.InstancePerCall);
            });

        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        try
        {
            await Runner.Run(endpointInstance)
                .ConfigureAwait(false);
        }
        finally
        {
            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }
    }
}