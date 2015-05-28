using System;
using NServiceBus;
using NServiceBus.Saga;

class Timeouts
{
    #region saga-with-timeout

    public class MySaga : Saga<MySagaData>,
        IAmStartedByMessages<Message1>,
        IHandleMessages<Message2>,
        IHandleTimeouts<MyCustomTimeout>
    {
        public override void ConfigureHowToFindSaga()
        {
            ConfigureMapping<Message2>(s => s.SomeID, m => m.SomeID);
        }

        public void Handle(Message1 message)
        {
            Data.SomeID = message.SomeID;
            RequestUtcTimeout<MyCustomTimeout>(TimeSpan.FromHours(1));
        }

        public void Handle(Message2 message)
        {
            Data.Message2Arrived = true;
            ReplyToOriginator(new AlmostDoneMessage
            {
                SomeID = Data.SomeID
            });
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



    public class MySagaData : IContainSagaData
    {
        public Guid Id { get; set; }
        public string Originator { get; set; }
        public string OriginalMessageId { get; set; }

        [Unique]
        public string SomeID { get; set; }

        public bool Message2Arrived { get; set; }
    }

    public class TiredOfWaitingForMessage2
    {
    }

    public class Message1
    {
        public string SomeID { get; set; }
    }

    public class MyCustomTimeout
    {
    }

    public class AlmostDoneMessage
    {
        public string SomeID { get; set; }
    }

    public class Message2
    {
        public string SomeID { get; set; }
        public string SomeData { get; set; }
    }

}