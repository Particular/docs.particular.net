namespace NServiceBus.RavenDB.Persistence.SagaPersister
{
    using System;

    class SagaUniqueIdentity
    {
        public string Id { get; set; }
        public Guid SagaId { get; set; }
        public object UniqueValue { get; set; }
        public string SagaDocId { get; set; }
    }
}