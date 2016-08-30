namespace Core3.Sagas.FindByProperty
{
    using NServiceBus.Saga;

    public class MySaga :
        Saga<MySagaData>,
        IAmStartedByMessages<MyMessage>
    {
        #region saga-find-by-property

        public override void ConfigureHowToFindSaga()
        {
            ConfigureMapping<MyMessage>(
                sagaData => sagaData.SomeId,
                message => message.SomeId);
        }

        #endregion

        public void Handle(MyMessage message)
        {
        }
    }

}