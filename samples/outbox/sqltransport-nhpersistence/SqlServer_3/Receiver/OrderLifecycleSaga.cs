using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class OrderLifecycleSaga : Saga<OrderLifecycleSagaData>,
    IAmStartedByMessages<OrderSubmitted>,
    IHandleTimeouts<OrderTimeout>
{
    static ILog log = LogManager.GetLogger<OrderLifecycleSaga>();

    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<OrderLifecycleSagaData> mapper)
    {
    }

    public async Task Handle(OrderSubmitted message, IMessageHandlerContext context)
    {
        Data.OrderId = message.OrderId;

        #region Timeout

        await RequestTimeout(context, TimeSpan.FromSeconds(5), new OrderTimeout());

        #endregion
    }

    public Task Timeout(OrderTimeout state, IMessageHandlerContext context)
    {
        log.Info("Got timeout");
        return Task.FromResult(0);
    }
}