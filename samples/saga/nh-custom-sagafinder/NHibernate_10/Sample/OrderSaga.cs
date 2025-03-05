

#region saga

using Microsoft.Extensions.Logging;

public class OrderSaga :
    Saga<OrderSagaData>,
    IAmStartedByMessages<StartOrder>,
    IHandleMessages<CompletePaymentTransaction>,
    IHandleMessages<CompleteOrder>
{
    private readonly ILogger<OrderSaga> logger;

    public OrderSaga(ILogger<OrderSaga> logger)
    {
        this.logger = logger;
    }

    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<OrderSagaData> mapper)
    {
        mapper.MapSaga(saga => saga.OrderId)
            .ToMessage<StartOrder>(msg => msg.OrderId)
            .ToMessage<CompleteOrder>(msg => msg.OrderId);
    }

    public Task Handle(StartOrder message, IMessageHandlerContext context)
    {
        Data.PaymentTransactionId = Guid.NewGuid().ToString();

        logger.LogInformation($"Saga with OrderId {Data.OrderId} received StartOrder with OrderId {message.OrderId}");
        var issuePaymentRequest = new IssuePaymentRequest
        {
            PaymentTransactionId = Data.PaymentTransactionId
        };
        return context.SendLocal(issuePaymentRequest);
    }

    public Task Handle(CompletePaymentTransaction message, IMessageHandlerContext context)
    {
        logger.LogInformation($"Transaction with Id {Data.PaymentTransactionId} completed for order id {Data.OrderId}");
        var completeOrder = new CompleteOrder
        {
            OrderId = Data.OrderId
        };
        return context.SendLocal(completeOrder);
    }

    public Task Handle(CompleteOrder message, IMessageHandlerContext context)
    {
        logger.LogInformation($"Saga with OrderId {Data.OrderId} received CompleteOrder with OrderId {message.OrderId}");
        MarkAsComplete();
        return Task.CompletedTask;
    }
}

#endregion