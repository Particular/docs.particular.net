using NServiceBus;
using NServiceBus.Features;

namespace Infrastructure;

public static class EndpointConfigurationExtensions
{
    public static void EnableAsyncApiSupport(this EndpointConfiguration endpointConfiguration)
    {
        endpointConfiguration.DisableFeature<AutoSubscribe>();
        endpointConfiguration.EnableFeature<AsyncApiFeature>();

        var conventions = endpointConfiguration.Conventions();
        conventions.Add(new PublishedEventsConvention());
        conventions.Add(new SubscribedEventsConvention());
    }
}