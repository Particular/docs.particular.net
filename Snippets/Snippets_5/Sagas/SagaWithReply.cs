
using System;
using NServiceBus.Saga;

public class SagaWithReply
{
    #region saga-with-reply

    public class MySaga : Saga<MySagaData>,
        IAmStartedByMessages<StartMessage>
    {

        public void Handle(StartMessage message)
        {
            Data.SomeID = message.SomeID;
            ReplyToOriginator(new AlmostDoneMessage
            {
                SomeID = Data.SomeID
            });
        }

        #endregion

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<MySagaData> mapper)
        {
        }
    }

    public class StartMessage 
    {
        public string SomeID { get; set; }
    }

    public class AlmostDoneMessage
    {
        public string SomeID { get; set; }
    }

    public class MySagaData : IContainSagaData
    {
        public Guid Id { get; set; }
        public string Originator { get; set; }
        public string OriginalMessageId { get; set; }

        [Unique]
        public string SomeID { get; set; }

        public bool Message2Arrived { get; set; }
    }

}
