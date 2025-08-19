using System;
using System.Threading.Tasks;
using NServiceBus;
using Microsoft.Extensions.Logging;
using NServiceBus.Persistence.Sql;

sealed class OrderLifecycleSaga(ILogger<OrderLifecycleSaga> logger) :
    SqlSaga<OrderLifecycleSaga.SagaData>,
    IAmStartedByMessages<OrderSubmitted>,
    IHandleTimeouts<OrderTimeout>
{
    protected override void ConfigureMapping(IMessagePropertyMapper mapper)
    {
        mapper.ConfigureMapping<OrderSubmitted>(_ => _.OrderId);
    }

    protected override string CorrelationPropertyName => nameof(SagaData.OrderId);

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