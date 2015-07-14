namespace Snippets5.Sagas.FindSagas
{
    using System;
    using NServiceBus.Saga;

    public class MySagaData : IContainSagaData
    {
        public Guid Id { get; set; }
        public string Originator { get; set; }
        public string OriginalMessageId { get; set; }
        public string SomeID { get; set; }
        public string SomeData { get; set; }
    }
}