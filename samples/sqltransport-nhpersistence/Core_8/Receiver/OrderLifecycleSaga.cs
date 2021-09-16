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

    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<OrderLifecycleSagaData> mapper)
    {
        mapper.ConfigureMapping<OrderSubmitted>(msg => msg.OrderId).ToSaga(saga => saga.OrderId);
    }

    public Task Handle(OrderSubmitted message, IMessageHandlerContext context)
    {
        Data.OrderId = message.OrderId;
        #region Timeout
        return RequestTimeout<OrderTimeout>(context, TimeSpan.FromSeconds(5));
        #endregion
    }

    public Task Timeout(OrderTimeout state, IMessageHandlerContext context)
    {
        log.Info("Got timeout");
        return Task.CompletedTask;
    }
}