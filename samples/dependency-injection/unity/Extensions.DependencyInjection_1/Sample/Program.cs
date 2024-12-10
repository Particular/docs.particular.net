﻿using System;
using System.Threading.Tasks;
using NServiceBus;
using Unity;
using Unity.Microsoft.DependencyInjection;

static class Program
{
    static async Task Main()
    {
        Console.Title = "Unity";

        #region ContainerConfiguration

        var endpointConfiguration = new EndpointConfiguration("Samples.Unity");

        var container = new UnityContainer();
        container.RegisterInstance(new MyService());

        endpointConfiguration.UseContainer<IUnityContainer>(new ServiceProviderFactory(container));

        #endregion

        endpointConfiguration.UseTransport<LearningTransport>();

        var endpointInstance = await Endpoint.Start(endpointConfiguration);
        var myMessage = new MyMessage();
        await endpointInstance.SendLocal(myMessage);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop();
    }
}