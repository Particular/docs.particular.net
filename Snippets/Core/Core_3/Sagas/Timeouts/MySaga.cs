namespace Core3.Sagas.Timeouts
{
    using System;
    using NServiceBus;
    using NServiceBus.Saga;

    #region saga-with-timeout

    public class MySaga :
        Saga<MySagaData>,
        IAmStartedByMessages<Message1>,
        IHandleMessages<Message2>,
        IHandleTimeouts<MyCustomTimeout>
    {
        public override void ConfigureHowToFindSaga()
        {
            ConfigureMapping<Message2>(
                sagaData => sagaData.SomeId,
                message => message.SomeId);
        }

        public void Handle(Message1 message)
        {
            Data.SomeId = message.SomeId;
            RequestUtcTimeout<MyCustomTimeout>(TimeSpan.FromHours(1));
        }

        public void Handle(Message2 message)
        {
            Data.Message2Arrived = true;
            var almostDoneMessage = new AlmostDoneMessage
            {
                SomeId = Data.SomeId
            };
            ReplyToOriginator(almostDoneMessage);
        }

        public void Timeout(MyCustomTimeout state)
        {
            if (!Data.Message2Arrived)
            {
                ReplyToOriginator(new TiredOfWaitingForMessage2());
            }
        }
    }

    #endregion
}