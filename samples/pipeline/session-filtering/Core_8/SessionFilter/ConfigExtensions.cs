using NServiceBus;

public static class ConfigExtensions
{
    #region config-extension
    public static void ApplySessionFilter(this EndpointConfiguration endpointConfiguration, ISessionKeyProvider sessionKeyProvider)
    {
        var pipeline = endpointConfiguration.Pipeline;
        pipeline.Register(new ApplySessionFilterHeader(sessionKeyProvider), "Adds session key to outgoing messages");
        pipeline.Register(new FilterIncomingMessages(sessionKeyProvider), "Filters out messages that don't match the current session key");
    }
    #endregion
}