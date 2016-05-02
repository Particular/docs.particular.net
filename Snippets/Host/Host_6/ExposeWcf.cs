using NServiceBus;

#region ExposeWCFService

public class CancelOrderService : WcfService<CancelOrder, ErrorCodes>
{
}

public class CancelOrderHandler : IHandleMessages<CancelOrder>
{
    IBus bus;

    public CancelOrderHandler(IBus bus)
    {
        this.bus = bus;
    }

    public void Handle(CancelOrder message)
    {
        // Write code here

        // Must return a status so that the WCF service has a return value
        bus.Return(ErrorCodes.Success);
    }
}

public enum ErrorCodes
{
    Success,
    Fail
}

public class CancelOrder : ICommand
{
    public int OrderId { get; set; }
}

#endregion