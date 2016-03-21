using NServiceBus;
using NServiceBus.Logging;

#region RequestDataMessageHandler

public class RequestDataMessageHandler : IHandleMessages<RequestDataMessage>

#endregion

{
    static ILog log = LogManager.GetLogger<RequestDataMessageHandler>();
    IBus bus;

    public RequestDataMessageHandler(IBus bus)
    {
        this.bus = bus;
    }

    public void Handle(RequestDataMessage message)
    {
        log.InfoFormat("Received request {0}.", message.DataId);
        log.InfoFormat("String received: {0}.", message.String);

        #region DataResponseReply

        DataResponseMessage response = new DataResponseMessage
                                       {
                                           DataId = message.DataId,
                                           String = message.String
                                       };

        bus.Reply(response);

        #endregion
    }
}