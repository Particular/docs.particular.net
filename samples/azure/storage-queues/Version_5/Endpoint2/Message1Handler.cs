using NServiceBus;
using NServiceBus.Logging;

public class Message1Handler : IHandleMessages<Message1>
{
    IBus bus;
    static ILog logger = LogManager.GetLogger<Message1Handler>();

    public Message1Handler(IBus bus)
    {
        this.bus = bus;
    }

    public void Handle(Message1 message)
    {
        logger.InfoFormat("Received Message1: {0}", message.Property);
        bus.Reply(new Message2
        {
            Property = "Hello from Endpoint2"
        });
    }
}