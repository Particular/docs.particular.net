using System;
using NServiceBus.Logging;
using NServiceBus.Saga;

public class OrderLifecycleSaga :
    Saga<OrderLifecycleSagaData>,
    IAmStartedByMessages<OrderSubmitted>,
    IHandleTimeouts<OrderTimeout>
{
    static ILog log = LogManager.GetLogger<OrderLifecycleSaga>();

    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<OrderLifecycleSagaData> mapper)
    {
    }

    public void Handle(OrderSubmitted message)
    {
        Data.OrderId = message.OrderId;

        #region Timeout
        RequestTimeout<OrderTimeout>(TimeSpan.FromSeconds(5));
        #endregion
    }

    public void Timeout(OrderTimeout state)
    {
        log.Info("Got timeout");
    }
}