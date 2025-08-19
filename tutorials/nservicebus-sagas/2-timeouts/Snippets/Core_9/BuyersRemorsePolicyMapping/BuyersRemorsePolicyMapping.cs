using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;

namespace Core_9.BuyersRemorsePolicyMapping;

#pragma warning disable 9113

#region BuyersRemorsePolicyMapping

class BuyersRemorsePolicy(ILogger<BuyersRemorsePolicy> logger) : Saga<BuyersRemorseData>,
    IAmStartedByMessages<PlaceOrder>
{
    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<BuyersRemorseData> mapper)
    {
        mapper.MapSaga(saga => saga.OrderId)
            .ToMessage<PlaceOrder>(message => message.OrderId);
    }

    public Task Handle(PlaceOrder message, IMessageHandlerContext context)
    {
        //To be replaced with business code
        return Task.CompletedTask;
    }
}

#endregion

#pragma warning restore 9113

internal class PlaceOrder
{
    public string OrderId { get; set; }
}

internal class BuyersRemorseData : ContainSagaData
{
    public string OrderId { get; set; }
}