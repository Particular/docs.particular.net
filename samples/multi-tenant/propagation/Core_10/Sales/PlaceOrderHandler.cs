using Microsoft.Extensions.Logging;

#region message-handler

class PlaceOrderHandler(ILogger<PlaceOrderHandler> logger) :
    IHandleMessages<PlaceOrder>
{

    public Task Handle(PlaceOrder message, IMessageHandlerContext context)
    {
        var tenant = context.MessageHeaders["tenant_id"];

        logger.LogInformation("Processing PlaceOrder message for tenant {Tenant}", tenant);

        return context.Publish(new OrderAccepted());
    }
}

#endregion