using Microsoft.Extensions.Logging;

#pragma warning disable NSB0006 // Message that starts the saga does not have a message mapping

namespace BuyersRemorsePolicyStartedByPlaceOrder;

#region BuyersRemorsePolicyStartedByPlaceOrder

class BuyersRemorsePolicy(ILogger<BuyersRemorsePolicy> logger) : Saga<BuyersRemorseData>, IAmStartedByMessages<PlaceOrder>
{
    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<BuyersRemorseData> mapper)
    {
        // TO BE IMPLEMENTED
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

#pragma warning restore NSB0006 // Message that starts the saga does not have a message mapping