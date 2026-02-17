using Microsoft.Extensions.Logging;

namespace BuyersRemorseCancelOrderHandling;

#region BuyersRemorseCancelOrderHandling

class BuyersRemorsePolicy(ILogger<BuyersRemorsePolicy> logger) : Saga<BuyersRemorseData>, IHandleMessages<CancelOrder>
{
    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<BuyersRemorseData> mapper)
    {
        mapper.MapSaga(saga => saga.OrderId)
            .ToMessage<PlaceOrder>(message => message.OrderId)
            .ToMessage<CancelOrder>(message => message.OrderId);
    }

    public Task Handle(CancelOrder message, IMessageHandlerContext context)
    {
        logger.LogInformation("Order #{orderId} was cancelled.", message.OrderId);

        //TODO: Possibly publish an OrderCancelled event?

        MarkAsComplete();

        return Task.CompletedTask;
    }
}

#endregion

internal class PlaceOrder
{
    public string? OrderId { get; internal set; }
}

internal class BuyersRemorseData : ContainSagaData
{
    public string? OrderId { get; set; }
}

internal class CancelOrder
{
    public string? OrderId { get; set; }
}