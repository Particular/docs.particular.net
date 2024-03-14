﻿using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.UsernameHeader.Endpoint2";
        var endpointConfiguration = new EndpointConfiguration("Samples.UsernameHeader.Endpoint2");
        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseTransport<LearningTransport>();

        #region component-registration-receiver

        endpointConfiguration.RegisterComponents(c =>
        {
            c.RegisterSingleton<IPrincipalAccessor>(new PrincipalAccessor());
            c.ConfigureComponent<SetCurrentPrincipalBasedOnHeaderMutator>(DependencyLifecycle.InstancePerCall);
        });

        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop();
    }
}