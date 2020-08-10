namespace Core8.Sagas.Timeouts
{
    using System;
    using NServiceBus;

    public class MySagaData :
        IContainSagaData
    {
        public Guid Id { get; set; }
        public string Originator { get; set; }
        public string OriginalMessageId { get; set; }

        public string SomeId { get; set; }

        public bool Message2Arrived { get; set; }
    }
}