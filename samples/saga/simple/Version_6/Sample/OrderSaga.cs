using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

#region thesaga
public class OrderSaga : Saga<OrderSagaData>,
    IAmStartedByMessages<StartOrder>,
    IHandleMessages<CompleteOrder>,
    IHandleTimeouts<CancelOrder>
{
    static ILog logger = LogManager.GetLogger<OrderSaga>();

    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<OrderSagaData> mapper)
    {
        mapper.ConfigureMapping<StartOrder>(message => message.OrderId)
                .ToSaga(sagaData => sagaData.OrderId);
        mapper.ConfigureMapping<CompleteOrder>(message => message.OrderId)
                .ToSaga(sagaData => sagaData.OrderId);
    }

    public async Task Handle(StartOrder message, IMessageHandlerContext context)
    {
        Data.OrderId = message.OrderId;
        logger.InfoFormat("Saga with OrderId {0} received StartOrder with OrderId {1}", Data.OrderId, message.OrderId);
        await context.SendLocal(new CompleteOrder
        {
            OrderId = Data.OrderId
        });
        await RequestTimeout<CancelOrder>(context, TimeSpan.FromMinutes(30));
    }

    public Task Handle(CompleteOrder message, IMessageHandlerContext context)
    {
        logger.InfoFormat("Saga with OrderId {0} received CompleteOrder with OrderId {1}", Data.OrderId, message.OrderId);
        MarkAsComplete();
        return Task.FromResult(0);
    }

    public Task Timeout(CancelOrder state, IMessageHandlerContext context)
    {
        logger.InfoFormat("Complete not received soon enough OrderId {0}", Data.OrderId);
        MarkAsComplete();
        return Task.FromResult(0);
    }
}
#endregion