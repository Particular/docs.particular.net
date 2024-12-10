using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class OrderLifecycleSaga :
    Saga<OrderLifecycleSagaData>,
    IAmStartedByMessages<OrderSubmitted>,
    IHandleTimeouts<OrderTimeout>
{
    static ILog log = LogManager.GetLogger<OrderLifecycleSaga>();
    
    public async Task Handle(OrderSubmitted message, IMessageHandlerContext context)
    {
        Data.OrderId = message.OrderId;

        await RequestTimeout<OrderTimeout>(context, TimeSpan.FromSeconds(5));
    }

    public Task Timeout(OrderTimeout state, IMessageHandlerContext context)
    {
        log.Info("Got timeout");

        return Task.CompletedTask;
    }

    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<OrderLifecycleSagaData> mapper)
    {
        mapper.ConfigureMapping<OrderSubmitted>(m => m.OrderId).ToSaga(s => s.OrderId);
    }
}