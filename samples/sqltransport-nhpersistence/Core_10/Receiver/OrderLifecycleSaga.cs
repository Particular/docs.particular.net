using System;
using System.Threading.Tasks;
using NServiceBus;
using Microsoft.Extensions.Logging;

public class OrderLifecycleSaga(ILogger<OrderLifecycleSaga> logger) :
    Saga<OrderLifecycleSagaData>,
    IAmStartedByMessages<OrderSubmitted>,
    IHandleTimeouts<OrderTimeout>
{
    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<OrderLifecycleSagaData> mapper)
    {
        mapper.MapSaga(s => s.OrderId).ToMessage<OrderSubmitted>(m => m.OrderId);
    }

    public Task Handle(OrderSubmitted message, IMessageHandlerContext context)
    {
        #region Timeout

        return RequestTimeout<OrderTimeout>(context, TimeSpan.FromSeconds(5));

        #endregion
    }

    public Task Timeout(OrderTimeout state, IMessageHandlerContext context)
    {
        logger.LogInformation("Got timeout");
        return Task.CompletedTask;
    }
}