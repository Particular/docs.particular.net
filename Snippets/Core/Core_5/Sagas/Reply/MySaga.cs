namespace Core5.Sagas.Reply
{
    using NServiceBus.Saga;

    #region saga-with-reply

    public class MySaga :
        Saga<MySagaData>,
        IAmStartedByMessages<StartMessage>
    {

        public void Handle(StartMessage message)
        {
            Data.SomeId = message.SomeId;
            var almostDoneMessage = new AlmostDoneMessage
            {
                SomeId = Data.SomeId
            };
            ReplyToOriginator(almostDoneMessage);
        }

        #endregion

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<MySagaData> mapper)
        {
        }
    }

}