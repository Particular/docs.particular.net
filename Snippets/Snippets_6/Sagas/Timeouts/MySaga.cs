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

        public async Task Handle(Message1 message, IMessageHandlerContext context)
        {
            Data.SomeID = message.SomeID;
            await RequestTimeoutAsync<MyCustomTimeout>(context, TimeSpan.FromHours(1));
        }

        public async Task Handle(Message2 message, IMessageHandlerContext context)
        {
            Data.Message2Arrived = true;
            await ReplyToOriginatorAsync(context, new AlmostDoneMessage
            {
                SomeID = Data.SomeID
            });
        }

        public async Task Timeout(MyCustomTimeout state, IMessageHandlerContext context)
        {
            if (!Data.Message2Arrived)
            {
                await ReplyToOriginatorAsync(context, new TiredOfWaitingForMessage2());
            }
        }
    }

    #endregion
}