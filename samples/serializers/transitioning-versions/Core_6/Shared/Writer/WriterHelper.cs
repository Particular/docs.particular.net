using NServiceBus;

#region writer-registration

public static class WriterHelper
{
    public static void AddMessageBodyWriter(this EndpointConfiguration endpointConfiguration)
    {
        var pipeline = endpointConfiguration.Pipeline;
        pipeline.Register(
            stepId: "OutgoingWriter",
            behavior: typeof(OutgoingWriter),
            description: "Logs the contents of each outgoing message and the TargetEndpoint");
        pipeline.Register(
            stepId: "IncomingWriter",
            behavior: typeof(IncomingWriter),
            description: "Logs the contents of each incoming message and the OriginatingEndpoint");
    }
}

#endregion