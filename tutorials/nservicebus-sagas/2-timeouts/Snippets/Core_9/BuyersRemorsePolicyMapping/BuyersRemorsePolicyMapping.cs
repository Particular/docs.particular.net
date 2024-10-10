using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;

namespace Core_9.BuyersRemorsePolicyMapping;

#region BuyersRemorsePolicyMapping

#pragma warning disable CS9113 // Parameter is unread.
class BuyersRemorsePolicy(ILogger<BuyersRemorsePolicy> logger) : Saga<BuyersRemorseState>,
#pragma warning restore CS9113 // Parameter is unread.
    IAmStartedByMessages<PlaceOrder>
{
    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<BuyersRemorseState> mapper)
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

internal class PlaceOrder
{
    public string OrderId { get; set; }
}

internal class BuyersRemorseState : ContainSagaData
{
    public string OrderId { get; set; }
}