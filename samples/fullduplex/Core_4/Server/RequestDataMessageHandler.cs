using NServiceBus;
using NServiceBus.Logging;

#region RequestDataMessageHandler

public class RequestDataMessageHandler :
    IHandleMessages<RequestDataMessage>

    #endregion

{
    static ILog log = LogManager.GetLogger(typeof(RequestDataMessageHandler));
    IBus bus;

    public RequestDataMessageHandler(IBus bus)
    {
        this.bus = bus;
    }

    public void Handle(RequestDataMessage message)
    {
        log.Info($"Received request {message.DataId}.");
        log.Info($"String received: {message.String}.");

        #region DataResponseReply

        var response = new DataResponseMessage
        {
            DataId = message.DataId,
            String = message.String
        };

        bus.Reply(response);

        #endregion
    }
}