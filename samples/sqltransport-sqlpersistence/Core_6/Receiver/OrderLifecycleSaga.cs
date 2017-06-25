using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Persistence.Sql;

#region Saga
public class OrderLifecycleSaga :
    SqlSaga<OrderLifecycleSaga.SagaData>,
    IAmStartedByMessages<OrderSubmitted>,
    IHandleTimeouts<OrderTimeout>
{
    static ILog log = LogManager.GetLogger<OrderLifecycleSaga>();

    protected override void ConfigureMapping(IMessagePropertyMapper mapper)
    {
        mapper.ConfigureMapping<OrderSubmitted>(_ => _.OrderId);
    }

    protected override string CorrelationPropertyName => nameof(SagaData.OrderId);

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

    public class SagaData :
        ContainSagaData
    {
        public Guid OrderId { get; set; }
    }
}
#endregion