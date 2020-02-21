﻿using System;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using NServiceBus;

static class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.Autofac";

        #region ContainerConfiguration

        var endpointConfiguration = new EndpointConfiguration("Samples.Autofac");

        var builder = new ContainerBuilder();

        builder.RegisterInstance(new MyService());

        var container = builder.Build();

        endpointConfiguration.UseContainer(new AutofacServiceProviderFactory(c =>
            c.RegisterInstance(new MyService())));

        #endregion

        endpointConfiguration.UseTransport<LearningTransport>();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        var myMessage = new MyMessage();
        await endpointInstance.SendLocal(myMessage)
            .ConfigureAwait(false);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}
