namespace Core5.UpgradeGuides._5to6
{
    using Core5.Sagas.SimpleSaga;
    using NServiceBus;
    using NServiceBus.Saga;


    public class OrderSaga :
        Saga<OrderSagaData>,
        IAmStartedByMessages<StartOrder>,
        IHandleMessages<CompleteOrder>
    {
        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<OrderSagaData> mapper)
        {
            mapper.ConfigureMapping<StartOrder>(message => message.OrderId)
                .ToSaga(sagaData => sagaData.OrderId);

            mapper.ConfigureMapping<CompleteOrder>(message => message.OrderId)
                .ToSaga(sagaData => sagaData.OrderId);
        }

        #region 5to6-NoSagaDataCorrelationNeeded

        public void Handle(StartOrder message)
        {
            Data.OrderId = message.OrderId;
            // The processing logic for the StartOrder message
        }

        #endregion

        public void Handle(CompleteOrder message)
        {
            // code to handle order completion
            MarkAsComplete();
        }
    }
}