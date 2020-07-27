namespace Core8.Sagas.FindByProperty
{
    using System.Threading.Tasks;
    using NServiceBus;

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

        public Task Handle(MyMessage message, IMessageHandlerContext context)
        {
            return Task.CompletedTask;
        }
    }

}