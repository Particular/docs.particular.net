using NServiceBus;

public static class RegisterBehaviors
{
    #region BehaviorConfigExtension

    public static void RegisterSigningBehaviors(this EndpointConfiguration endpointConfiguration)
    {
        var pipeline = endpointConfiguration.Pipeline;
        pipeline.Register(new MessageSigningBehavior(), "Signs outgoing messages with a cryptographic signature");
        pipeline.Register(new SignatureVerificationBehavior(), "Validates the cryptographic signature on incoming messages");
    }

    #endregion
}
