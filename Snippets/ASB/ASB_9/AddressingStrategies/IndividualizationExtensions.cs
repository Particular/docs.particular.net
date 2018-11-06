using NServiceBus;
using NServiceBus.Configuration.AdvancedExtensibility;

#region custom-individualization-strategy-extension

public static class AzureServiceBusIndividualizationStrategyExtensions
{
    public static AzureServiceBusIndividualizationSettings MySetting(this AzureServiceBusIndividualizationSettings extensionPoint)
    {
        var settings = extensionPoint.GetSettings();
        return extensionPoint;
    }
}

#endregion