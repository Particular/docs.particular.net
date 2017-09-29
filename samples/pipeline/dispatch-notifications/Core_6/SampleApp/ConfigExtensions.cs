using System.Collections.Generic;
using NServiceBus;
using NServiceBus.Configuration.AdvanceExtensibility;
using NServiceBus.Features;

#region config-extensions
static class ConfigExtensions
{
    public static void NotifyDispatch(this EndpointConfiguration config, IWatchDispatches watch)
    {
        var settings = config.GetSettings();
        settings.EnableFeatureByDefault<DispatchNotificationFeature>();
        settings.GetOrCreate<List<IWatchDispatches>>().Add(watch);
    }
}
#endregion