using System.Collections.Generic;
using NServiceBus;
using NServiceBus.Configuration.AdvancedExtensibility;
using NServiceBus.Features;

#region config-extensions
static class ConfigExtensions
{
    public static void NotifyDispatch(this EndpointConfiguration endpointConfiguration, IWatchDispatches watch)
    {
        var settings = endpointConfiguration.GetSettings();
        settings.EnableFeatureByDefault<DispatchNotificationFeature>();
        settings.GetOrCreate<List<IWatchDispatches>>().Add(watch);
    }
}
#endregion