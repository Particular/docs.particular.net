namespace Snippets5.Sagas.Timeouts
{
    using System;
    using NServiceBus.Saga;

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