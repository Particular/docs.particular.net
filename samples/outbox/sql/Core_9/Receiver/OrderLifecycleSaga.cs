using System;
using System.Threading.Tasks;
using NServiceBus;
using Microsoft.Extensions.Logging;
using NServiceBus.Persistence.Sql;

sealed class OrderLifecycleSaga(ILogger<OrderLifecycleSaga> logger) :
    Saga<OrderLifecycleSaga.SagaData>,
    IAmStartedByMessages<OrderSubmitted>,
    IHandleTimeouts<OrderTimeout>
{
    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<SagaData> mapper)
    {
        mapper.MapSaga(saga => saga.OrderId)
            .ToMessage<OrderSubmitted>(msg => msg.OrderId);
    }

    #region Timeout

    public Task Handle(OrderSubmitted message, IMessageHandlerContext context)
    {
        var orderTimeout = new OrderTimeout();
        return RequestTimeout(context, TimeSpan.FromSeconds(5), orderTimeout);
    }

    #endregion

    public Task Timeout(OrderTimeout state, IMessageHandlerContext context)
    {
        logger.LogInformation("Got timeout");
        return Task.CompletedTask;
    }

    public class SagaData :
        ContainSagaData
    {
        public Guid OrderId { get; set; }
    }
}