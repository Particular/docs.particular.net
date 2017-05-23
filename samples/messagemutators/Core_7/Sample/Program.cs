﻿using System;
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
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseTransport<LearningTransport>();

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
        await Runner.Run(endpointInstance)
            .ConfigureAwait(false);
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}