namespace Core5.Sagas.FindByProperty
{
    using NServiceBus.Saga;

    public class MySaga :
        Saga<MySagaData>,
        IAmStartedByMessages<MyMessage>
    {
        #region saga-find-by-property

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<MySagaData> mapper)
        {
            mapper.ConfigureMapping<MyMessage>(message => message.SomeId)
                .ToSaga(sagaData => sagaData.SomeId);
        }

        #endregion

        public void Handle(MyMessage message)
        {
        }
    }

}