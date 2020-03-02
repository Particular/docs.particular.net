using NServiceBus;
using NServiceBus.Configuration.AdvancedExtensibility;

#region custom-sanitization-strategy-extension

public static class AzureServiceBusSanitizationStrategyExtensions
{
    public static AzureServiceBusSanitizationSettings MySetting(this AzureServiceBusSanitizationSettings extensionPoint)
    {
        var settings = extensionPoint.GetSettings();
        return extensionPoint;
    }
}

#endregion