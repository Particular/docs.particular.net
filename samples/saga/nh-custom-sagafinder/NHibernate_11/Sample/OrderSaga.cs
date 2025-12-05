

#region saga

using Microsoft.Extensions.Logging;

public class OrderSaga(ILogger<OrderSaga> logger) :
    Saga<OrderSagaData>,
    IAmStartedByMessages<StartOrder>,
    IHandleMessages<CompletePaymentTransaction>,
    IHandleMessages<CompleteOrder>
{
    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<OrderSagaData> mapper)
    {
        mapper.MapSaga(saga => saga.OrderId)
            .ToMessage<StartOrder>(msg => msg.OrderId)
            .ToMessage<CompleteOrder>(msg => msg.OrderId);

        mapper.ConfigureFinderMapping<CompletePaymentTransaction, CompletePaymentTransactionSagaFinder>();
    }

    public Task Handle(StartOrder message, IMessageHandlerContext context)
    {
        Data.PaymentTransactionId = Guid.NewGuid().ToString();

        logger.LogInformation("Saga with OrderId {SagaOrderId} received StartOrder with OrderId {MessageOrderId}", Data.OrderId, message.OrderId);
        var issuePaymentRequest = new IssuePaymentRequest
        {
            PaymentTransactionId = Data.PaymentTransactionId
        };
        return context.SendLocal(issuePaymentRequest);
    }

    public Task Handle(CompletePaymentTransaction message, IMessageHandlerContext context)
    {
        logger.LogInformation("Transaction with Id {PaymentTransactionId} completed for order id {OrderId}", Data.PaymentTransactionId, Data.OrderId);
        var completeOrder = new CompleteOrder
        {
            OrderId = Data.OrderId
        };
        return context.SendLocal(completeOrder);
    }

    public Task Handle(CompleteOrder message, IMessageHandlerContext context)
    {
        logger.LogInformation("Saga with OrderId {SagaOrderId} received CompleteOrder with OrderId {MessageOrderId}", Data.OrderId, message.OrderId);
        MarkAsComplete();
        return Task.CompletedTask;
    }
}

#endregion