using Microsoft.Extensions.Logging;

namespace BuyersRemorsePolicyMapping;

#region BuyersRemorsePolicyMapping

class BuyersRemorsePolicy(ILogger<BuyersRemorsePolicy> logger) : Saga<BuyersRemorseData>, IAmStartedByMessages<PlaceOrder>
{
    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<BuyersRemorseData> mapper)
    {
        mapper.MapSaga(saga => saga.OrderId)
            .ToMessage<PlaceOrder>(message => message.OrderId);
    }

    public Task Handle(PlaceOrder message, IMessageHandlerContext context)
    {
        logger.LogInformation("Received PlaceOrder, OrderId = {orderId}", message.OrderId);

        Data.OrderId = message.OrderId;

        return Task.CompletedTask;
    }
}

#endregion

internal class PlaceOrder
{
    public string? OrderId { get; set; }
}

internal class BuyersRemorseData : ContainSagaData
{
    public string? OrderId { get; set; }
}