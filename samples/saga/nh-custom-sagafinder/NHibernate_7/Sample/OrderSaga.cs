using NServiceBus;
using NServiceBus.Logging;
using System;
using System.Threading.Tasks;

#region TheSagaNHibernate

public class OrderSaga :
    Saga<OrderSagaData>,
    IAmStartedByMessages<StartOrder>,
    IHandleMessages<PaymentTransactionCompleted>,
    IHandleMessages<CompleteOrder>
{
    static ILog log = LogManager.GetLogger<OrderSaga>();

    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<OrderSagaData> mapper)
    {
        // NOP
    }

    public Task Handle(StartOrder message, IMessageHandlerContext context)
    {
        Data.OrderId = message.OrderId;
        Data.PaymentTransactionId = Guid.NewGuid().ToString();

        log.Info($"Saga with OrderId {Data.OrderId} received StartOrder with OrderId {message.OrderId}");
        var issuePaymentRequest = new IssuePaymentRequest
        {
            PaymentTransactionId = Data.PaymentTransactionId
        };
        return context.SendLocal(issuePaymentRequest);
    }

    public Task Handle(PaymentTransactionCompleted message, IMessageHandlerContext context)
    {
        log.Info($"Transaction with Id {Data.PaymentTransactionId} completed for order id {Data.OrderId}");
        var completeOrder = new CompleteOrder
        {
            OrderId = Data.OrderId
        };
        return context.SendLocal(completeOrder);
    }

    public Task Handle(CompleteOrder message, IMessageHandlerContext context)
    {
        log.Info($"Saga with OrderId {Data.OrderId} received CompleteOrder with OrderId {message.OrderId}");
        MarkAsComplete();

        return Task.CompletedTask;
    }

}

#endregion