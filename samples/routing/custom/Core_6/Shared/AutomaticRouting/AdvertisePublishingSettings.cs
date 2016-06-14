using System;
using NServiceBus.Configuration.AdvanceExtensibility;
using NServiceBus.Settings;

public class AdvertisePublishingSettings : ExposeSettings
{
    public AdvertisePublishingSettings(SettingsHolder settings) : base(settings)
    {
    }

    public void AdvertisePublishing(params Type[] publishedTypes)
    {
        this.GetSettings().Set("NServiceBus.AutomaticRouting.PublishedTypes", publishedTypes);
    }
}