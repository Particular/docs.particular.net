namespace SimplSaga
{
    using System;
    using NServiceBus;
    using NServiceBus.Saga;

    #region simple-saga

    public class OrderSaga : Saga<OrderSagaData>,
                            IAmStartedByMessages<StartOrder>,
                            IHandleMessages<CompleteOrder>
    {
        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<OrderSagaData> mapper)
        {
            mapper.ConfigureMapping<CompleteOrder>(s => s.OrderId)
                    .ToSaga(m => m.OrderId);
        }

        public void Handle(StartOrder message)
        {
            Data.OrderId = message.OrderId;
        }

        public void Handle(CompleteOrder message)
        {
            // code to handle order completion
            MarkAsComplete();
        }
    }

    #endregion


    public class StartOrder
    {
        public string OrderId { get; set; }
    }
    public class CompleteOrder
    {
        public string OrderId { get; set; }
        public string SomeData { get; set; }
    }
    #region simple-saga-data
    public class OrderSagaData : IContainSagaData
    {
        public Guid Id { get; set; }
        public string Originator { get; set; }
        public string OriginalMessageId { get; set; }
        [Unique]
        public string OrderId { get; set; }
    }
    #endregion
}
