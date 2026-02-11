using Microsoft.Extensions.Logging;
using NServiceBus;

namespace Core_9.EmptyBuyersRemorsePolicy;

#pragma warning disable 1998, 9113

#region EmptyBuyersRemorsePolicy
class BuyersRemorsePolicy(ILogger<BuyersRemorsePolicy> logger) : Saga<BuyersRemorseData>
{
    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<BuyersRemorseData> mapper)
    {
        // TO BE IMPLEMENTED
    }
}

public class BuyersRemorseData : ContainSagaData
{
    public string OrderId { get; set; }
}
#endregion

#pragma warning restore 1998, 9113