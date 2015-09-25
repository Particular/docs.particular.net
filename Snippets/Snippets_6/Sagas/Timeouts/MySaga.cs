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

        public async Task Handle(Message1 message)
        {
            Data.SomeID = message.SomeID;
            await RequestTimeoutAsync<MyCustomTimeout>(TimeSpan.FromHours(1));
        }

        public async Task Handle(Message2 message)
        {
            Data.Message2Arrived = true;
            await ReplyToOriginatorAsync(new AlmostDoneMessage
            {
                SomeID = Data.SomeID
            });
        }

        public async Task Timeout(MyCustomTimeout state)
        {
            if (!Data.Message2Arrived)
            {
                await ReplyToOriginatorAsync(new TiredOfWaitingForMessage2());
            }
        }
    }

    #endregion
}