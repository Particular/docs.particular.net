using Microsoft.Extensions.Logging;

namespace BuyersRemorseTimeoutHandling;

#region BuyersRemorseTimeoutHandling

class BuyersRemorsePolicy(ILogger<BuyersRemorsePolicy> logger) : Saga<BuyersRemorseData>, IHandleTimeouts<BuyersRemorseIsOver>
{
    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<BuyersRemorseData> mapper)
    {
        //Omitted for clarity
    }

    public async Task Timeout(BuyersRemorseIsOver state, IMessageHandlerContext context)
    {
        logger.LogInformation("Cooling down period for order #{orderId} has elapsed.", Data.OrderId);

        var orderPlaced = new OrderPlaced
        {
            OrderId = Data.OrderId
        };

        await context.Publish(orderPlaced);

        MarkAsComplete();
    }
}

#endregion

internal class OrderPlaced
{
    public string? OrderId { get; set; }
}

internal class BuyersRemorseIsOver { }


internal class BuyersRemorseData : ContainSagaData
{
    public string? OrderId { get; set; }
}