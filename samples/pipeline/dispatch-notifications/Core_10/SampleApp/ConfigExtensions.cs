using System.Collections.Generic;
using NServiceBus;
using NServiceBus.Configuration.AdvancedExtensibility;
using NServiceBus.Features;

#region config-extensions
static class ConfigExtensions
{
    public static void NotifyDispatch(this EndpointConfiguration endpointConfiguration, IDispatchNotifier watch)
    {
        var settings = endpointConfiguration.GetSettings();
        settings.EnableFeature<DispatchNotificationFeature>();
        settings.GetOrCreate<List<IDispatchNotifier>>().Add(watch);
    }
}
#endregion