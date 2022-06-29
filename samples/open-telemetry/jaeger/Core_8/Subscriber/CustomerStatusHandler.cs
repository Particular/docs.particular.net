using Shared;

namespace Subscriber;

public class CustomerStatusHandler : IHandleMessages<OrderReceived>
{
    public async Task Handle(OrderReceived message, IMessageHandlerContext context)
    {
        await Task.Delay(25, context.CancellationToken);
    }
}