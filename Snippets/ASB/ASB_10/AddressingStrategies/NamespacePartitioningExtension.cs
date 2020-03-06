using NServiceBus;
using NServiceBus.Configuration.AdvancedExtensibility;

#region custom-namespace-partitioning-strategy-extension

public static class AzureServiceBusNamespacePartitioningStrategyExtensions
{
    public static AzureServiceBusNamespacePartitioningSettings MySetting(this AzureServiceBusNamespacePartitioningSettings extensionPoint)
    {
        var settings = extensionPoint.GetSettings();
        return extensionPoint;
    }
}

#endregion