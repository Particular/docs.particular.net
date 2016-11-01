using System;
using NServiceBus.Persistence.MongoDB;
using NServiceBus.Saga;

#region sagadata

public class OrderSagaData :
    IContainSagaData
{
    public Guid Id { get; set; }
    public string Originator { get; set; }
    public string OriginalMessageId { get; set; }

    [DocumentVersion]
    public int Version { get; set; }

    [Unique]
    public Guid OrderId { get; set; }
    public string OrderDescription { get; set; }
}
#endregion