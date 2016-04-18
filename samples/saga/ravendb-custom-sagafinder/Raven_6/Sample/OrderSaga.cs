using NServiceBus;
using NServiceBus.Logging;
using System;
using System.Threading.Tasks;

#region TheSagaRavenDB

public class OrderSaga : Saga<OrderSagaData>,
    IAmStartedByMessages<StartOrder>,
    IHandleMessages<PaymentTransactionCompleted>,
    IHandleMessages<CompleteOrder>
{
    static ILog logger = LogManager.GetLogger<OrderSaga>();

    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<OrderSagaData> mapper)
    {
        //NOP
    }

    public Task Handle(StartOrder message, IMessageHandlerContext context)
    {
        Data.OrderId = message.OrderId;
        Data.PaymentTransactionId = Guid.NewGuid().ToString();

        logger.InfoFormat("Saga with OrderId {0} received StartOrder with OrderId {1}", Data.OrderId, message.OrderId);
        return context.SendLocal(new IssuePaymentRequest
                      {
                          PaymentTransactionId = Data.PaymentTransactionId
                      });
    }

    public Task Handle(PaymentTransactionCompleted message, IMessageHandlerContext context)
    {
        logger.InfoFormat("Transaction with Id {0} completed for order id {1}", Data.PaymentTransactionId, Data.OrderId);
        return context.SendLocal(new CompleteOrder
                      {
                          OrderId = Data.OrderId
                      });
    }

    public Task Handle(CompleteOrder message, IMessageHandlerContext context)
    {
        logger.InfoFormat("Saga with OrderId {0} received CompleteOrder with OrderId {1}", Data.OrderId, message.OrderId);
        MarkAsComplete();

        return Task.FromResult(0);
    }
}

#endregion
