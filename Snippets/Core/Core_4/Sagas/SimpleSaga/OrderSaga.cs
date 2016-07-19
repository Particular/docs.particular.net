namespace Core4.Sagas.SimpleSaga
{
    using NServiceBus;
    using NServiceBus.Saga;

    #region simple-saga

    public class OrderSaga :
        Saga<OrderSagaData>,
        IAmStartedByMessages<StartOrder>,
        IHandleMessages<CompleteOrder>
    {
        public override void ConfigureHowToFindSaga()
        {
            ConfigureMapping<StartOrder>(message => message.OrderId)
                .ToSaga(sagaData => sagaData.OrderId);

            ConfigureMapping<CompleteOrder>(message => message.OrderId)
                .ToSaga(sagaData => sagaData.OrderId);
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

}