
using System;
using NServiceBus;
using NServiceBus.Saga;

public class SagaFindByExpression
{
    public class MySaga : Saga<MySagaData>,
        IAmStartedByMessages<Message1>,
        IHandleMessages<Message2>
    {
        #region saga-find-by-expression

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<MySagaData> mapper)
        {
            mapper.ConfigureMapping<Message2>(m => m.Part1 + "_" + m.Part2)
                .ToSaga(m => m.SomeID);
        }

        #endregion

        public void Handle(Message1 message)
        {
            // code to handle Message1
        }

        public void Handle(Message2 message)
        {
            // code to handle Message2
        }
    }

    public class Message2
    {
        public string Part1 { get; set; }
        public string Part2 { get; set; }
    }

    public class Message1
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
    }

}