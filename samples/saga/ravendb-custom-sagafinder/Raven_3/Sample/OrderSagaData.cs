using System;
using NServiceBus.Saga;
using Raven.Client.UniqueConstraints;

#region OrderSagaDataRavenDB

public class OrderSagaData :
    IContainSagaData
{
    public Guid Id { get; set; }
    public string Originator { get; set; }
    public string OriginalMessageId { get; set; }

    [UniqueConstraint(CaseInsensitive = true)]
    public string OrderId { get; set; }

    [UniqueConstraint(CaseInsensitive = true)]
    public string PaymentTransactionId { get; set; }
}

#endregion