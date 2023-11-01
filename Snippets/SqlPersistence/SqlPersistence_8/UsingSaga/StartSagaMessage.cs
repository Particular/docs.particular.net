namespace SqlPersistence_1.UsingSaga
{
    using System;
    using NServiceBus;

    public class StartSagaMessage :
        IMessage
    {
        public Guid CorrelationProperty { get; set; }
        public Guid TransitionalCorrelationProperty { get; set; }
    }
}