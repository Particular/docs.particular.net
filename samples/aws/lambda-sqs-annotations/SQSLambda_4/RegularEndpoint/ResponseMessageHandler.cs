using NServiceBus.Logging;

public class ResponseMessageHandler : IHandleMessages<ResponseMessage>
{
    static readonly ILog Log = LogManager.GetLogger<ResponseMessageHandler>();

    public Task Handle(ResponseMessage message, IMessageHandlerContext context)
    {
        Log.Info($"Handling {nameof(ResponseMessage)} in RegularEndpoint");
        return Task.CompletedTask;
    }
}