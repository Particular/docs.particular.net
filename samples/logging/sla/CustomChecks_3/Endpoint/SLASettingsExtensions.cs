using System;
using NServiceBus;
using NServiceBus.Configuration.AdvancedExtensibility;

public static class SLASettingsExtensions
{
    public static void EndpointSLA(this EndpointConfiguration configuration, TimeSpan timeToBreachSLA,
        TimeSpan timeToNotifyAboutSLABreachToOccur)
    {
        configuration.EnableFeature<EstimateTimeToBreachSLA>();
        var settings = configuration.GetSettings().GetOrCreate<SLASettings>();
        settings.TimeToBreachSLA = timeToBreachSLA;
        settings.TimeToNotifyAboutSLABreachToOccur = timeToNotifyAboutSLABreachToOccur;
    }
}