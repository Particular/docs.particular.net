using Messages;
using Microsoft.Extensions.Logging;

namespace Billing;

public class CustomerStatusPolicy(ILogger<CustomerStatusPolicy> logger) :
    Saga<CustomerStatusPolicyData>,
    IAmStartedByMessages<OrderBilled>,
    IHandleTimeouts<CustomerStatusPolicy.OrderExpired>
{

    //values hardcoded for simplicity
    const int preferredStatusAmount = 250;
    readonly TimeSpan orderExpiryTimeout = TimeSpan.FromSeconds(10);

    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<CustomerStatusPolicyData> mapper)
    {
        mapper.MapSaga(saga => saga.CustomerId)
            .ToMessage<OrderBilled>(message => message.CustomerId);
    }

    public async Task Handle(OrderBilled message, IMessageHandlerContext context)
    {
        logger.LogInformation("Customer {CustomerId} submitted order of {OrderValue}", Data.CustomerId, message.OrderValue);

        Data.RunningTotal += message.OrderValue;
        await CheckForPreferredStatus(context);

        await RequestTimeout(context, orderExpiryTimeout, new OrderExpired() { Amount = message.OrderValue });
    }

    public async Task Timeout(OrderExpired timeout, IMessageHandlerContext context)
    {
        logger.LogInformation("Customer {CustomerId} order of {Amount} timed out.", Data.CustomerId, timeout.Amount);
        Data.RunningTotal -= timeout.Amount;
        await CheckForPreferredStatus(context);
    }

    async Task CheckForPreferredStatus(IMessageHandlerContext context)
    {
        if (!Data.PreferredStatus && Data.RunningTotal >= preferredStatusAmount)
        {
            Data.PreferredStatus = true;
            await context.Publish<CustomerHasBecomePreferred>(s => s.CustomerId = Data.CustomerId);
        }
        else if (Data.PreferredStatus && Data.RunningTotal < preferredStatusAmount)
        {
            Data.PreferredStatus = false;
            await context.Publish<CustomerHasBecomeNonPreferred>(s => s.CustomerId = Data.CustomerId);
        }
    }

    public class OrderExpired
    {
        public decimal Amount { get; set; }
    }
}

public class CustomerStatusPolicyData : ContainSagaData
{
    public string? CustomerId { get; set; }
    public decimal RunningTotal { get; set; }
    public bool PreferredStatus { get; set; }
}