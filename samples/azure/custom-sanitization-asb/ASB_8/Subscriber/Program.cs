using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static void Main()
    {
        MainAsync().GetAwaiter().GetResult();
    }

    static async Task MainAsync()
    {
        Console.Title = "Samples.ASB.Serialization.Subscriber";
        var endpointConfiguration = new EndpointConfiguration("Samples.ASB.Serialization.Subscriber");
        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        var connectionString = Environment.GetEnvironmentVariable("AzureServiceBus.ConnectionString");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new Exception("Could not read the 'AzureServiceBus.ConnectionString' environment variable. Check the sample prerequisites.");
        }
        transport.ConnectionString(connectionString);
        var topology = transport.UseForwardingTopology();

        #region CustomSanitization

        var sanitization = transport.Sanitization();
        sanitization.UseStrategy<Sha1Sanitization>();

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
        recoverability.DisableLegacyRetriesSatellite();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Subscriber is ready to receive events");
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}