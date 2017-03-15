using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Persistence.Sql;

[SqlSaga(correlationProperty:nameof(SagaData.OrderId))]
public class OrderLifecycleSaga :
    SqlSaga<OrderLifecycleSaga.SagaData>,
    IAmStartedByMessages<OrderSubmitted>,
    IHandleTimeouts<OrderTimeout>
{
    static ILog log = LogManager.GetLogger<OrderLifecycleSaga>();

    protected override void ConfigureMapping(MessagePropertyMapper<SagaData> mapper)
    {
        mapper.MapMessage<OrderSubmitted>(_ => _.OrderId);
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
        log.Info("Got timeout");
        return Task.CompletedTask;
    }

    public class SagaData :
        ContainSagaData
    {
        public virtual string OrderId { get; set; }
    }

}