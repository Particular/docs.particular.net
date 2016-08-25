namespace Core3.Sagas.FindSagas
{
    using System;
    using NServiceBus.Saga;

    public class MySagaData :
        IContainSagaData
    {
        public string SomeID { get; set; }
        public string SomeData { get; set; }
        public Guid Id { get; set; }
        public string Originator { get; set; }
        public string OriginalMessageId { get; set; }
    }
}