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
        logger.InfoFormat($"Saga with OrderId {Data.OrderId} received StartOrder with OrderId {message.OrderId}");
        var completeOrder = new CompleteOrder
        {
            OrderId = Data.OrderId
        };
        await context.SendLocal(completeOrder)
            .ConfigureAwait(false);
        await RequestTimeout<CancelOrder>(context, TimeSpan.FromMinutes(30))
            .ConfigureAwait(false);
    }

    public Task Handle(CompleteOrder message, IMessageHandlerContext context)
    {
        logger.InfoFormat($"Saga with OrderId {Data.OrderId} received CompleteOrder with OrderId {message.OrderId}");
        MarkAsComplete();
        return Task.FromResult(0);
    }

    public Task Timeout(CancelOrder state, IMessageHandlerContext context)
    {
        logger.InfoFormat($"Complete not received soon enough OrderId {Data.OrderId}");
        MarkAsComplete();
        return Task.FromResult(0);
    }
}
#endregion