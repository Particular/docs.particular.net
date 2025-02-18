using Microsoft.Extensions.Logging;
using NServiceBus;

namespace Core_9.EmptyBuyersRemorsePolicy;

#pragma warning disable 1998

#region EmptyBuyersRemorsePolicy
#pragma warning disable CS9113 // Parameter is unread.
class BuyersRemorsePolicy(ILogger<BuyersRemorsePolicy> logger) : Saga<BuyersRemorseData>
#pragma warning restore CS9113 // Parameter is unread.
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

#pragma warning restore 1998