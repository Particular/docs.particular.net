using NServiceBus;
using NServiceBus.Features;

public static class NServiceBusFeatures
{
    public static void DisableNotUsedFeatures(this EndpointConfiguration endpointConfiguration)
    {
        endpointConfiguration.DisableFeature<Sagas>();
        endpointConfiguration.Recoverability().Delayed(s=>s.NumberOfRetries(0));
        endpointConfiguration.DisableFeature<TimeoutManager>();
    }
}
