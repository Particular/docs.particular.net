using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;

namespace Core_9.BuyersRemorseTimeoutHandling;

#region BuyersRemorseTimeoutHandling

class BuyersRemorsePolicy(ILogger<BuyersRemorsePolicy> logger) : Saga<BuyersRemorseData>,
    IHandleTimeouts<BuyersRemorseIsOver>
{
    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<BuyersRemorseData> mapper)
    {
        //Omitted for clarity
    }

    public async Task Timeout(BuyersRemorseIsOver timeout, IMessageHandlerContext context)
    {
        logger.LogInformation("Cooling down period for order #{OrderId} has elapsed.", Data.OrderId);

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
    public object OrderId { get; set; }
}

internal class BuyersRemorseIsOver
{
}

internal class PlaceOrder
{
}

internal class BuyersRemorseData : ContainSagaData
{
    public object OrderId { get; set; }
}