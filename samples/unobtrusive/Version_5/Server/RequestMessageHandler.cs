using Messages;
using NServiceBus;
using NServiceBus.Logging;

public class RequestMessageHandler : IHandleMessages<Request>
{
    static ILog log = LogManager.GetLogger<RequestMessageHandler>();
    IBus bus;

    public RequestMessageHandler(IBus bus)
    {
        this.bus = bus;
    }

    public void Handle(Request message)
    {
        log.Info("Request received with id:" + message.RequestId);

        bus.Reply(new Response
                        {
                            ResponseId = message.RequestId
                        });
    }
}
