using System;
using NServiceBus;
using NServiceBus.Configuration.AdvancedExtensibility;

public static class LockRenewalConfiguration
{
    public static void LockRenewal(this EndpointConfiguration endpointConfiguration, Action<LockRenewalOptions> action)
    {
        var options = new LockRenewalOptions();

        action(options);

        var settings = endpointConfiguration.GetSettings();

        settings.Set<LockRenewalOptions>(options);
    }
}