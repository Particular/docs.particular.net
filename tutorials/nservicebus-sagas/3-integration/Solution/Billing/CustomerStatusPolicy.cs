using Messages;
using NServiceBus;
using NServiceBus.Logging;
using System;
using System.Threading.Tasks;

namespace Billing
{
    public class CustomerStatusPolicy :
        Saga<CustomerStatusPolicy.CustomerStatusState>,
        IAmStartedByMessages<OrderBilled>,
        IHandleTimeouts<CustomerStatusPolicy.OrderExpired>
    {
        static ILog log = LogManager.GetLogger<CustomerStatusPolicy>();

        //values hardcoded for simplicity
        const int preferredStatusAmount = 250;
        TimeSpan orderExpiryTimeout = TimeSpan.FromSeconds(10);

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<CustomerStatusState> mapper)
        {
            mapper.ConfigureMapping<OrderBilled>(message => message.CustomerId).ToSaga(saga => saga.CustomerId);
        }

        public async Task Handle(OrderBilled message, IMessageHandlerContext context)
        {
            Data.CustomerId = message.CustomerId;

            log.Info($"Customer {Data.CustomerId} submitted order of {message.OrderValue}");

            Data.RunningTotal += message.OrderValue;
            await CheckForPreferredStatus(context);

            await RequestTimeout(context, orderExpiryTimeout, new OrderExpired() { Amount = message.OrderValue });
        }

        public async Task Timeout(OrderExpired timeout, IMessageHandlerContext context)
        {
            log.Info($"Customer {Data.CustomerId} order of {timeout.Amount} timed out.");
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

        public class CustomerStatusState : ContainSagaData
        {
            public string CustomerId { get; set; }
            public decimal RunningTotal { get; set; }
            public bool PreferredStatus { get; set; }
        }

        public class OrderExpired
        {
            public decimal Amount { get; set; }
        }
    }
}