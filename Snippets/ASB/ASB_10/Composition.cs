using NServiceBus;
using NServiceBus.Transport.AzureServiceBus;

class Composition
{
    void HierarchyComposition(EndpointConfiguration endpointConfiguration)
    {
        #region asb-hierarchy-composition

        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        var composition = transport.Composition();
        var hierarchy = composition.UseStrategy<HierarchyComposition>();
        hierarchy.PathGenerator(
            pathGenerator: entityName =>
            {
                return "production/tenant1/";
            });

        #endregion
    }

    void CustomComposition(EndpointConfiguration endpointConfiguration)
    {
        #region asb-custom-composition-config

        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        var composition = transport.Composition();
        composition.UseStrategy<CustomComposition>();

        #endregion
    }
}

#region asb-custom-composition-strategy

class CustomComposition :
    ICompositionStrategy
{
    public string GetEntityPath(string entityName, EntityType entityType)
    {
        return "path/to/entity";
    }
}

#endregion
