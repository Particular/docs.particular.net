using NServiceBus;

#region AzureMultiHost_PingHandler

public class PingHandler :
    IHandleMessages<Ping>
{
    IBus bus;

    public PingHandler(IBus bus)
    {
        this.bus = bus;
    }

    public void Handle(Ping message)
    {
        VerificationLogger.Write("Receiver", "Got Ping and will reply with Pong");
        bus.Reply(new Pong());
    }
}

#endregion
