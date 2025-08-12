using NServiceBus.Features;

namespace AsyncAPI.Feature;

public static class EndpointConfigurationExtensions
{
    #region EnableAsyncApiSupport
    public static void EnableAsyncApiSupport(
        this EndpointConfiguration endpointConfiguration)
    {
        endpointConfiguration.DisableFeature<AutoSubscribe>();
        endpointConfiguration.EnableFeature<AsyncApiFeature>();

        var conventions = endpointConfiguration.Conventions();
        conventions.Add(new PublishedEventsConvention());
        conventions.Add(new SubscribedEventsConvention());
    }
    #endregion
}