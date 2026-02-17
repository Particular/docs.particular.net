using Microsoft.Extensions.Logging;

namespace EmptyBuyersRemorsePolicy;

#pragma warning disable CS9113 // Parameter is unread.

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
    public string? OrderId { get; set; }
}
#endregion

#pragma warning restore CS9113 // Parameter is unread.
