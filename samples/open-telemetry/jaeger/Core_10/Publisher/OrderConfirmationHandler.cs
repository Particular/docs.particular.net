using Shared;

public class OrderConfirmationHandler : IHandleMessages<ConfirmOrderMessage>, IHandleMessages<DeclineOrderMessage>
{
    public async Task Handle(ConfirmOrderMessage message, IMessageHandlerContext context)
    {
        await Task.Delay(25, context.CancellationToken);
        Console.WriteLine($"Order {message.OrderId} was confirmed.");
    }

    public async Task Handle(DeclineOrderMessage message, IMessageHandlerContext context)
    {
        await Task.Delay(25, context.CancellationToken);
        Console.WriteLine($"Order {message.OrderId} was declined.");
    }
}