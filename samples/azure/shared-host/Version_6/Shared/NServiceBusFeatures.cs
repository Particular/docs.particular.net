using NServiceBus;
using NServiceBus.Features;

public static class NServiceBusFeatures
{
    public static void DisableNotUsedFeatures(this EndpointConfiguration endpointConfiguration)
    {
        endpointConfiguration.DisableFeature<Sagas>();
        endpointConfiguration.DisableFeature<SecondLevelRetries>();
        endpointConfiguration.DisableFeature<TimeoutManager>();
    }
}
