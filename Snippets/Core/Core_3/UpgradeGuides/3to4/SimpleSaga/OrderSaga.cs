namespace Core3.UpgradeGuides._3to4.SimpleSaga
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
            ConfigureMapping<StartOrder>(
                sagaData => sagaData.OrderId,
                message => message.OrderId);

            ConfigureMapping<CompleteOrder>(
                sagaData => sagaData.OrderId,
                message => message.OrderId);
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