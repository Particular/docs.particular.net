using Microsoft.Extensions.Logging;

public class OrderLifecycleSaga(ILogger<OrderLifecycleSaga> logger) :
    Saga<OrderLifecycleSagaData>,
    IAmStartedByMessages<OrderSubmitted>,
    IHandleTimeouts<OrderTimeout>
{

    public async Task Handle(OrderSubmitted message, IMessageHandlerContext context)
    {
        Data.OrderId = message.OrderId;

        await RequestTimeout<OrderTimeout>(context, TimeSpan.FromSeconds(5));
    }

    public Task Timeout(OrderTimeout state, IMessageHandlerContext context)
    {
        logger.LogInformation("Order {OrderId} has timed out", Data.OrderId);
        return Task.CompletedTask;
    }

    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<OrderLifecycleSagaData> mapper)
    {
        mapper.MapSaga(saga => saga.OrderId)
            .ToMessage<OrderSubmitted>(m => m.OrderId);
    }
}