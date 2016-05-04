using System.Diagnostics.CodeAnalysis;
using NServiceBus;
using NServiceBus.AzureServiceBus;
using NServiceBus.AzureServiceBus.Addressing;

[SuppressMessage("ReSharper", "UnusedMember.Local")]
public class Sanitization
{
    void EndpointOrientedTopologySanitization(EndpointConfiguration endpointConfiguration)
    {
        #region asb-endpointorientedtopology-sanitization

        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();

        transport.Sanitization().UseStrategy<EndpointOrientedTopologySanitization>();

        #endregion
    }


    void CustomSanitization(EndpointConfiguration endpointConfiguration)
    {
        #region asb-custom-sanitization

        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();

        transport.Sanitization().UseStrategy<CustomSanitization>();

        #endregion
    }

}

#region asb-custom-sanitization-strategy

class CustomSanitization : ISanitizationStrategy
{
    public string Sanitize(string entityPathOrName, EntityType entityType)
    {
        // apply sanitization on entityPathOrName
        return entityPathOrName;
    }
}

#endregion
