using System;
using NServiceBus;

public class SagaState :
    ContainSagaData
{
    public Guid LongProcessingId { get; set; }
}