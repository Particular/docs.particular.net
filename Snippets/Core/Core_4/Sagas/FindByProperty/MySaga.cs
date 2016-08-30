namespace Core4.Sagas.FindByProperty
{
    using NServiceBus.Saga;

    public class MySaga :
        Saga<MySagaData>,
        IAmStartedByMessages<MyMessage>
    {
        #region saga-find-by-property

        public override void ConfigureHowToFindSaga()
        {
            ConfigureMapping<MyMessage>(message => message.SomeId)
                .ToSaga(sagaData => sagaData.SomeId);
        }

        #endregion

        public void Handle(MyMessage message)
        {
        }
    }

}