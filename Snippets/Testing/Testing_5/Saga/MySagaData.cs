namespace Snippets5.UnitTesting.Saga
{
    using System;
    using NServiceBus.Saga;

    public class MySagaData : IContainSagaData
    {
        public Guid Id { get; set; }
        public string Originator { get; set; }
        public string OriginalMessageId { get; set; }
    }
}