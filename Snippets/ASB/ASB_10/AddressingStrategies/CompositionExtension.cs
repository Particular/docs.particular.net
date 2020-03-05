using NServiceBus;
using NServiceBus.Configuration.AdvancedExtensibility;

#region custom-composition-strategy-extension

public static class AzureServiceBusCompositionStrategyExtensions
{
    public static AzureServiceBusCompositionSettings MySetting(this AzureServiceBusCompositionSettings extensionPoint)
    {
        var settings = extensionPoint.GetSettings();
        return extensionPoint;
    }
}

#endregion
