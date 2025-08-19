using Shared;

public class OrderValidationHandler : IHandleMessages<OrderReceived>
{
    public async Task Handle(OrderReceived message, IMessageHandlerContext context)
    {
        await Task.Delay(50, context.CancellationToken);
        if (Random.Shared.NextDouble() >= 0.5)
        {
            Console.WriteLine($"Confirming order {message.OrderId}");
            await context.Reply(new ConfirmOrderMessage() { OrderId = message.OrderId });
        }
        else
        {
            Console.WriteLine($"Declining order {message.OrderId}");
            await context.Reply(new DeclineOrderMessage() { OrderId = message.OrderId });
        }
    }
}