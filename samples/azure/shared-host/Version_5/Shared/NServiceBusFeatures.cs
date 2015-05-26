using NServiceBus;
using NServiceBus.Features;

public static class NServiceBusFeatures
{
    public static void DisableNotUsedFeatures(this BusConfiguration configuration)
    {
        configuration.DisableFeature<Sagas>();
        configuration.DisableFeature<SecondLevelRetries>();
        configuration.DisableFeature<TimeoutManager>();
    }
}
