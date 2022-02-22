namespace Core7.Sagas.Reply
{
    using NServiceBus;
    using System.Threading.Tasks;

    #region saga-with-reply

    public class MySaga :
        Saga<MySagaData>,
        IAmStartedByMessages<StartMessage>
    {
        public Task Handle(StartMessage message, IMessageHandlerContext context)
        {
            var almostDoneMessage = new AlmostDoneMessage
            {
                SomeId = Data.SomeId
            };
            return ReplyToOriginator(context, almostDoneMessage);
        }

        #endregion

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<MySagaData> mapper)
        {
            mapper.MapSaga(saga => saga.SomeId)
                .ToMessage<StartMessage>(msg => msg.SomeId);
        }

    }

}
