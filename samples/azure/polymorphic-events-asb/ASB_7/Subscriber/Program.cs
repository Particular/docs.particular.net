﻿using System;
using System.Threading.Tasks;
using Events;
using NServiceBus;
using NServiceBus.Features;

class Program
{
    static void Main()
    {
        MainAsync().GetAwaiter().GetResult();
    }

    static async Task MainAsync()
    {
        Console.Title = "Samples.ASB.Polymorphic.Subscriber";
        var endpointConfiguration = new EndpointConfiguration("Samples.ASB.Polymorphic.Subscriber");
        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        var connectionString = Environment.GetEnvironmentVariable("AzureServiceBus.ConnectionString");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new Exception("Could not read the 'AzureServiceBus.ConnectionString' environment variable. Check the sample prerequisites.");
        }
        transport.ConnectionString(connectionString);
        var topology = transport.UseEndpointOrientedTopology();
        transport.Sanitization().UseStrategy<ValidateAndHashIfNeeded>();

        #region RegisterPublisherNames

        topology.RegisterPublisher(typeof(BaseEvent), "Samples.ASB.Polymorphic.Publisher");

        #endregion

        endpointConfiguration.SendFailedMessagesTo("error");

        #region DisableAutoSubscripton

        endpointConfiguration.DisableFeature<AutoSubscribe>();

        #endregion

        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        var recoverability = endpointConfiguration.Recoverability();
        recoverability.Delayed(
            customizations: settings =>
            {
                settings.NumberOfRetries(0);
            });

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        try
        {
            #region ControledSubscriptions

            await endpointInstance.Subscribe<BaseEvent>()
                .ConfigureAwait(false);

            #endregion

            Console.WriteLine("Subscriber is ready to receive events");
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
        finally
        {
            await endpointInstance.Unsubscribe<BaseEvent>().ConfigureAwait(false);
            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }
    }
}