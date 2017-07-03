namespace Core6.Sagas.Timeouts
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus;

    #region saga-with-timeout

    public class MySaga :
        Saga<MySagaData>,
        IAmStartedByMessages<Message1>,
        IHandleMessages<Message2>,
        IHandleTimeouts<MyCustomTimeout>
    {
        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<MySagaData> mapper)
        {
            mapper.ConfigureMapping<Message2>(message => message.SomeId)
                .ToSaga(sagaData => sagaData.SomeId);
        }

        public Task Handle(Message1 message, IMessageHandlerContext context)
        {
            return RequestTimeout<MyCustomTimeout>(context, TimeSpan.FromHours(1));
        }

        public Task Handle(Message2 message, IMessageHandlerContext context)
        {
            Data.Message2Arrived = true;
            var almostDoneMessage = new AlmostDoneMessage
            {
                SomeId = Data.SomeId
            };
            return RequestTimeout(context, TimeSpan.FromHours(1), almostDoneMessage);
        }

        public Task Timeout(MyCustomTimeout state, IMessageHandlerContext context)
        {
            if (Data.Message2Arrived)
            {
                return Task.CompletedTask;
            }
            return RequestTimeout(context, TimeSpan.FromHours(1), new TiredOfWaitingForMessage2());
        }
    }

    #endregion
}