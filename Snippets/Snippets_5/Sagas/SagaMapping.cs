using System;
using NServiceBus;
using NServiceBus.Saga;

public class SagaMapping
{

    // startcode ConfigureHowToFindSaga
    public class MySaga : Saga<MySagaData>,
        IAmStartedByMessages<Message1>,
        IHandleMessages<Message2>
    {
        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<MySagaData> mapper)
        {
            mapper.ConfigureMapping<Message2>(s => s.SomeID)
                .ToSaga(m => m.SomeID);
        }

        // endcode
        public void Handle(Message1 message)
        {
        }

        public void Handle(Message2 message)
        {
        }
    }

    public class Message1
    {
    }

    public class Message2
    {
        public Guid SomeID { get; set; }
    }

    public class MySagaData : IContainSagaData
    {
        public Guid Id { get; set; }
        public string Originator { get; set; }
        public string OriginalMessageId { get; set; }
        public Guid SomeID { get; set; }
    }
}