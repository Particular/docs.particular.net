using System;
using System.Threading.Tasks;
using Events;
using NServiceBus;
using NServiceBus.Features;

class Program
{
    static async Task Main()
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
        var sanitization = transport.Sanitization();
        sanitization.UseStrategy<ValidateAndHashIfNeeded>();

        #region RegisterPublisherNames

        topology.RegisterPublisher(typeof(BaseEvent), "Samples.ASB.Polymorphic.Publisher");

        #endregion

        #region DisableAutoSubscripton

        endpointConfiguration.DisableFeature<AutoSubscribe>();

        #endregion

        endpointConfiguration.UseSerialization<NewtonsoftSerializer>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        var recoverability = endpointConfiguration.Recoverability();
        recoverability.Delayed(
            customizations: settings => { settings.NumberOfRetries(0); });

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        #region ControledSubscriptions

        await endpointInstance.Subscribe<BaseEvent>()
            .ConfigureAwait(false);

        #endregion

        Console.WriteLine("Subscriber is ready to receive events");
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Unsubscribe<BaseEvent>()
            .ConfigureAwait(false);
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}