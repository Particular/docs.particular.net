using Microsoft.Extensions.Logging;
using NServiceBus;

public static class ConfigExtensions
{
    #region config-extension
    public static void ApplySessionFilter(this EndpointConfiguration endpointConfiguration)
    {
        var pipeline = endpointConfiguration.Pipeline;
        pipeline.Register(typeof(ApplySessionFilterHeader), "Adds session key to outgoing messages");
        pipeline.Register(typeof(FilterIncomingMessages), "Filters out messages that don't match the current session key");
    }
    #endregion
}