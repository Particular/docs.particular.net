using NServiceBus;

#region AzureMultiHost_SendPingCommand

public class SendPing :
    IWantToRunWhenBusStartsAndStops
{
    IBus bus;

    public SendPing(IBus bus)
    {
        this.bus = bus;
    }

    public void Start()
    {
        VerificationLogger.Write("Sender", "Pinging Receiver");
        bus.Send(new Ping());
    }

    public void Stop()
    {
    }
}

#endregion
