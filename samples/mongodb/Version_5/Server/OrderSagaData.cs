using System;
using NServiceBus.MongoDB;
using NServiceBus.Saga;

#region sagadata

public class OrderSagaData : 
    IHaveDocumentVersion, 
    IContainSagaData
{
    public Guid Id { get; set; }
    public string Originator { get; set; }
    public string OriginalMessageId { get; set; }

    [Unique]
    public Guid OrderId { get; set; }

    public string OrderDescription { get; set; }


    public int DocumentVersion { get; set; }
}
#endregion