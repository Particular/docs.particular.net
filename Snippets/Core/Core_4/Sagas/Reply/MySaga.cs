namespace Core4.Sagas.Reply
{
    using NServiceBus.Saga;

    #region saga-with-reply

    public class MySaga :
        Saga<MySagaData>,
        IAmStartedByMessages<StartMessage>
    {

        public void Handle(StartMessage message)
        {
            Data.SomeID = message.SomeID;
            var almostDoneMessage = new AlmostDoneMessage
            {
                SomeID = Data.SomeID
            };
            ReplyToOriginator(almostDoneMessage);
        }

        #endregion

    }

}