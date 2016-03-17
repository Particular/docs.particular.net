namespace Snippets6.UnitTesting.Saga
{
    using System;
    using NServiceBus;

    public class MySagaData : IContainSagaData
    {
        public Guid Id { get; set; }
        public string Originator { get; set; }
        public string OriginalMessageId { get; set; }
    }
}