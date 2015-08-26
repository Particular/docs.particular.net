namespace Store.Sales
{
    using System;
    using System.Diagnostics;
    using Common;
    using Messages.Commands;
    using Messages.Events;
    using NServiceBus;
    using NServiceBus.Saga;

    public class ProcessOrderSaga : Saga<ProcessOrderSaga.OrderData>,
                                    IAmStartedByMessages<SubmitOrder>,
                                    IHandleMessages<CancelOrder>,
                                    IHandleTimeouts<ProcessOrderSaga.BuyersRemorseIsOver>
    {
        public void Handle(SubmitOrder message)
        {
            if (DebugFlagMutator.Debug)
            {
                Debugger.Break();
            }

            Data.OrderNumber = message.OrderNumber;
            Data.ProductIds = message.ProductIds;
            Data.ClientId = message.ClientId;

            RequestTimeout(TimeSpan.FromSeconds(20), new BuyersRemorseIsOver());
            Console.WriteLine("Starting cool down period for order #{0}.", Data.OrderNumber);
        }

        public void Timeout(BuyersRemorseIsOver state)
        {
            if (DebugFlagMutator.Debug)
            {
                Debugger.Break();
            }

            Bus.Publish<OrderAccepted>(e =>
                {
                    e.OrderNumber = Data.OrderNumber;
                    e.ProductIds = Data.ProductIds;
                    e.ClientId = Data.ClientId;
                });

            MarkAsComplete();

            Console.WriteLine("Cooling down period for order #{0} has elapsed.", Data.OrderNumber);
        }

        public void Handle(CancelOrder message)
        {
            if (DebugFlagMutator.Debug)
            {
                   Debugger.Break();
            }

            MarkAsComplete();

            Bus.Publish<OrderCancelled>(o =>
                {
                    o.OrderNumber = message.OrderNumber;
                    o.ClientId = message.ClientId;
                });

            Console.WriteLine("Order #{0} was cancelled.", message.OrderNumber);
        }

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<OrderData> mapper)
        {
            mapper.ConfigureMapping<SubmitOrder>(m => m.OrderNumber)
                .ToSaga(s => s.OrderNumber);
            mapper.ConfigureMapping<CancelOrder>(m => m.OrderNumber)
                .ToSaga(s => s.OrderNumber);
        }

        public class OrderData : ContainSagaData
        {
            public int OrderNumber { get; set; }
            public string[] ProductIds { get; set; }
            public string ClientId { get; set; }
        }

        public class BuyersRemorseIsOver
        {
        }

    }

    
}