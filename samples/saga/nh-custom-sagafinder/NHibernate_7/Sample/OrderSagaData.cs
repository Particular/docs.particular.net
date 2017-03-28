using System;
using NServiceBus;

public class OrderSagaData :
    IContainSagaData
{
    public virtual Guid Id { get; set; }
    public virtual string Originator { get; set; }
    public virtual string OriginalMessageId { get; set; }
    public virtual string OrderId { get; set; }
    public virtual string PaymentTransactionId { get; set; }
}