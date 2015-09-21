namespace Snippets6.Sagas.Timeouts
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus;

    #region saga-with-timeout

    public class MySaga : Saga<MySagaData>,
        IAmStartedByMessages<Message1>,
        IHandleMessages<Message2>,
        IHandleTimeouts<MyCustomTimeout>
    {
        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<MySagaData> mapper)
        {
            mapper.ConfigureMapping<Message2>(s => s.SomeID)
                .ToSaga(m => m.SomeID);
        }

        public Task Handle(Message1 message)
        {
            Data.SomeID = message.SomeID;
            RequestTimeout<MyCustomTimeout>(TimeSpan.FromHours(1));
            return Task.FromResult(0);
        }

        public Task Handle(Message2 message)
        {
            Data.Message2Arrived = true;
            ReplyToOriginator(new AlmostDoneMessage
            {
                SomeID = Data.SomeID
            });
            return Task.FromResult(0);
        }

        public Task Timeout(MyCustomTimeout state)
        {
            if (!Data.Message2Arrived)
            {
                ReplyToOriginator(new TiredOfWaitingForMessage2());
            }
            return Task.FromResult(0);
        }
    }

    #endregion
}