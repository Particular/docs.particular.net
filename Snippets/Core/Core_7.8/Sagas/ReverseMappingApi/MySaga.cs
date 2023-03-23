namespace Core7.Sagas.ReverseMapping
{
    using System.Threading.Tasks;
    using NServiceBus;

    public class MySaga : Saga<MySagaData>, IAmStartedByMessages<MyFirstMessage>
    {
        #region reverse-saga-mapping-api

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<MySagaData> mapper)
        {
            mapper.MapSaga(saga => saga.SomeId)
                .ToMessage<MyFirstMessage>(msg => msg.SomeId)
                .ToMessage<MySecondMessage>(msg => msg.SomeOtherId)
                .ToMessageHeader<MyThirdMessage>("SomeHeaderKey");
        }

        #endregion

        public Task Handle(MyFirstMessage message, IMessageHandlerContext context)
        {
            return Task.CompletedTask;
        }
    }
}