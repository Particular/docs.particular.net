using NServiceBus.Configuration.AdvancedExtensibility;
using NServiceBus.Features;
using YamlDotNet.Serialization;

namespace AsyncAPI.Feature;

public static class EndpointConfigurationExtensions
{
    #region EnableAsyncApiSupport
    public static void EnableAsyncApiSupport(
        this EndpointConfiguration endpointConfiguration)
    {        
        endpointConfiguration.EnableFeature<AsyncApiFeature>();
    }
    #endregion
}