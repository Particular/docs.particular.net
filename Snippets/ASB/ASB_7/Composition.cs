using NServiceBus;
using NServiceBus.AzureServiceBus;
using NServiceBus.AzureServiceBus.Addressing;

class Composition
{
    void HierarchyComposition(EndpointConfiguration endpointConfiguration)
    {
        #region asb-hierarchy-composition

        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        var hierarchy = transport.Composition().UseStrategy<HierarchyComposition>();
        hierarchy.PathGenerator(entityName => "production/tenant1/" );

        #endregion
    }

    void CustomComposition(EndpointConfiguration endpointConfiguration)
    {
        #region asb-custom-composition-config

        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        transport.Composition().UseStrategy<CustomComposition>();

        #endregion
    }
}

#region asb-custom-composition-strategy

class CustomComposition :
    ICompositionStrategy
{
    public string GetEntityPath(string entityname, EntityType entityType)
    {
        return "path/to/entity";
    }
}

#endregion