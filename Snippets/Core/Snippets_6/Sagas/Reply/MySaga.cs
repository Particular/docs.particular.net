namespace Core6.Sagas.Reply
{
    using System.Threading.Tasks;
    using NServiceBus;

    #region saga-with-reply

    public class MySaga : Saga<MySagaData>,
        IAmStartedByMessages<StartMessage>
    {

        public async Task Handle(StartMessage message, IMessageHandlerContext context)
        {
            Data.SomeID = message.SomeID;

            await ReplyToOriginator(context, new AlmostDoneMessage
            {
                SomeID = Data.SomeID
            });
        }

        #endregion

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<MySagaData> mapper)
        {
        }
        
    }

}