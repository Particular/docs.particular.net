class CreateOrderHandler : IHandleMessages<CreateOrder>
{
    #region custom-activity-in-handler
    public async Task Handle(CreateOrder message, IMessageHandlerContext context)
    {
        using var activity = CustomActivitySources.Main.StartActivity("Billing Order");

        if (message.SimulateFailure)
        {
            throw new MyBusinessException{ ReasonCode = 1};
        }

        Console.WriteLine($"Billing order {message.OrderId}");
        activity?.AddTag("sample.billing.system", "paypal");
        // Calculate order cost
        await context.SendLocal(new BillOrder { OrderId = message.OrderId });

        Console.WriteLine($"Shipping order {message.OrderId}");
        await context.SendLocal(new ShipOrder { OrderId = message.OrderId });
    }
    #endregion
}