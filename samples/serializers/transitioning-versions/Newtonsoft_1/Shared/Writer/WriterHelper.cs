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
            description: "OutgoingWriter");
        pipeline.Register(
            stepId: "IncomingWriter",
            behavior: typeof(IncomingWriter),
            description: "IncomingWriter");
    }
}

#endregion