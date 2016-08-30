namespace Core4.Sagas.Reply
{
    using System;
    using NServiceBus.Saga;

    public class MySagaData :
        IContainSagaData
    {
        public Guid Id { get; set; }
        public string Originator { get; set; }
        public string OriginalMessageId { get; set; }

        [Unique]
        public string SomeId { get; set; }

        public bool Message2Arrived { get; set; }
    }
}