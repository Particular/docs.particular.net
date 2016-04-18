namespace Core6.Sagas.Timeouts
{
    using System;
    using NServiceBus;

    public class MySagaData : IContainSagaData
    {
        public Guid Id { get; set; }
        public string Originator { get; set; }
        public string OriginalMessageId { get; set; }

        public string SomeID { get; set; }

        public bool Message2Arrived { get; set; }
    }
}