using NServiceBus;
using NServiceBus.Features;

public static class NServiceBusFeatures
{
    public static void DisableNotUsedFeatures(this EndpointConfiguration endpointConfiguration)
    {
        endpointConfiguration.DisableFeature<Sagas>();
        var recoverability = endpointConfiguration.Recoverability();
        recoverability.Delayed(
            customizations: settings =>
            {
                settings.NumberOfRetries(0);
            });
        endpointConfiguration.DisableFeature<TimeoutManager>();
    }
}