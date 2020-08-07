namespace Core7.Sagas.FindByHeader
{
    using System.Threading.Tasks;
    using NServiceBus;

    public class MySaga : Saga<MySagaData>, IAmStartedByMessages<MyMessage>
    {
        #region saga-find-by-message-header

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<MySagaData> mapper)
        {
            mapper.ConfigureHeaderMapping<MyMessage>("HeaderName")
                .ToSaga(saga => saga.SomeId);
        }

        #endregion

        public Task Handle(MyMessage message, IMessageHandlerContext context)
        {
            return Task.CompletedTask;
        }
    }
}