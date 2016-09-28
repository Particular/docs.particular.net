using System.Threading.Tasks;
using NServiceBus;

#region ExposeWCFService

public class CancelOrderService :
    WcfService<CancelOrder, ErrorCodes>
{
}

public class CancelOrderHandler :
    IHandleMessages<CancelOrder>
{
    public Task Handle(CancelOrder message, IMessageHandlerContext context)
    {
        // code to handle the message

        // return a status so that the WCF service has a return value
        return context.Reply(ErrorCodes.Success);
    }
}

public enum ErrorCodes
{
    Success,
    Fail
}

public class CancelOrder :
    ICommand
{
    public int OrderId { get; set; }
}

#endregion