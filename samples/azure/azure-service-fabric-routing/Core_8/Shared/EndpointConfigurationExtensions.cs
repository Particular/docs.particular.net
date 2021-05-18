using System;
using Microsoft.ServiceFabric.Data;
using NServiceBus;
using NServiceBus.Persistence.ServiceFabric;

public static class EndpointConfigurationExtensions
{
    public static RoutingSettings<AzureServiceBusTransport> ApplyCommonConfiguration(this EndpointConfiguration endpointConfiguration, IReliableStateManager stateManager)
    {
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.AuditProcessedMessagesTo("audit");
        endpointConfiguration.UseSerialization<NewtonsoftSerializer>();
        endpointConfiguration.EnableInstallers();
        var persistence = endpointConfiguration.UsePersistence<ServiceFabricPersistence>();
        persistence.StateManager(stateManager);

        var connectionString = Environment.GetEnvironmentVariable("AzureServiceBus.ConnectionString");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new Exception("Could not read the 'AzureServiceBus.ConnectionString' environment variable. Check the sample prerequisites.");
        }

        var transport = new AzureServiceBusTransport(connectionString);
        var routing = endpointConfiguration.UseTransport(transport);
        
        return routing;
    }
}
