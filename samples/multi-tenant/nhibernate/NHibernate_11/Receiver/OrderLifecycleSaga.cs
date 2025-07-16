using NServiceBus.Logging;

public class OrderLifecycleSaga :
    Saga<OrderLifecycleSagaData>,
    IAmStartedByMessages<OrderSubmitted>,
    IHandleTimeouts<OrderTimeout>
{
    static readonly ILog log = LogManager.GetLogger<OrderLifecycleSaga>();

    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<OrderLifecycleSagaData> mapper)
    {
        mapper.MapSaga(saga => saga.OrderId)
            .ToMessage<OrderSubmitted>(message => message.OrderId);
    }

    public async Task Handle(OrderSubmitted message, IMessageHandlerContext context)
    {
        Data.OrderId = message.OrderId;

        await RequestTimeout<OrderTimeout>(context, TimeSpan.FromSeconds(5));
    }

    public Task Timeout(OrderTimeout state, IMessageHandlerContext context)
    {
        log.Info($"Order {Data.OrderId} has timed out");

        return Task.CompletedTask;
    }
}