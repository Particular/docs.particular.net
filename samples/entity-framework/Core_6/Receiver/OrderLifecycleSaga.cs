using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class OrderLifecycleSaga :
    Saga<OrderLifecycleSagaData>,
    IAmStartedByMessages<OrderSubmitted>,
    IHandleTimeouts<OrderTimeout>
{
    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<OrderLifecycleSagaData> mapper)
    {
        mapper.ConfigureMapping<OrderSubmitted>(m => m.OrderId)
            .ToSaga(s => s.OrderId);
    }

    public async Task Handle(OrderSubmitted message, IMessageHandlerContext context)
    {
        var orderTimeout = new OrderTimeout();
        await RequestTimeout(context, TimeSpan.FromSeconds(5), orderTimeout)
            .ConfigureAwait(false);

        var orderAccepted = new OrderAccepted
        {
            OrderId = message.OrderId,
        };
        await context.Reply(orderAccepted)
            .ConfigureAwait(false);
    }

    public Task Timeout(OrderTimeout state, IMessageHandlerContext context)
    {
        return context.SendLocal(new CompleteOrder
        {
            OrderId = Data.OrderId
        });
    }
}