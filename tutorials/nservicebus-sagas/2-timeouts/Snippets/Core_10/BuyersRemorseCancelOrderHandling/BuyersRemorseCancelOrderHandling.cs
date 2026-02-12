using Microsoft.Extensions.Logging;

namespace BuyersRemorseCancelOrderHandling;

#region BuyersRemorseCancelOrderHandling

class BuyersRemorsePolicy(ILogger<BuyersRemorsePolicy> logger) : Saga<BuyersRemorseData>,
    IAmStartedByMessages<PlaceOrder>,
    IHandleMessages<CancelOrder>,
    IHandleTimeouts<BuyersRemorseIsOver>
{
  protected override void ConfigureHowToFindSaga(SagaPropertyMapper<BuyersRemorseData> mapper)
    {
        mapper.MapSaga(saga => saga.OrderId)
            .ToMessage<PlaceOrder>(message => message.OrderId)
            .ToMessage<CancelOrder>(message => message.OrderId);
    }

    public Task Handle(CancelOrder message, IMessageHandlerContext context)
    {
        logger.LogInformation("Order #{OrderId} was cancelled.", message.OrderId);

        //TODO: Possibly publish an OrderCancelled event?

        MarkAsComplete();

        return Task.CompletedTask;
    }
}

#endregion

internal interface IHandleTimeouts<T>
{
}

internal interface IHandleMessages<T>
{
}

internal interface IAmStartedByMessages<T>
{
}

public interface IMessageHandlerContext
{
}

internal class Saga<T>
{
    protected virtual void ConfigureHowToFindSaga(SagaPropertyMapper<T> mapper) { }

    protected void MarkAsComplete()
    {
    }
}

internal class SagaPropertyMapper<TSagaData>
{
    internal SagaPropertyMapper<TSagaData> MapSaga(Func<TSagaData, object> p)
    {
        return this;
    }

    internal SagaPropertyMapper<TSagaData> ToMessage<T>(Func<T, object> p)
    {
        return this;
    }
}

internal class OrderPlaced
{
    public object OrderId { get; set; }
}

internal class BuyersRemorseIsOver
{
}

internal class PlaceOrder
{
    public object OrderId { get; internal set; }
}

internal class BuyersRemorseData
{
    public object OrderId { get; set; }
}

internal class CancelOrder
{
    public object OrderId { get; set; }
}