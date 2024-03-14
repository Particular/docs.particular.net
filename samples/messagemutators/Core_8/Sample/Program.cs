using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.MessageMutator;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.MessageMutators";
        var endpointConfiguration = new EndpointConfiguration("Samples.MessageMutators");
        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        endpointConfiguration.UseTransport(new LearningTransport());

        #region ComponentRegistration

        endpointConfiguration.RegisterMessageMutator(new ValidationMessageMutator());
        endpointConfiguration.RegisterMessageMutator(new TransportMessageCompressionMutator());

        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration);
        await Runner.Run(endpointInstance);
        await endpointInstance.Stop();
    }
}
