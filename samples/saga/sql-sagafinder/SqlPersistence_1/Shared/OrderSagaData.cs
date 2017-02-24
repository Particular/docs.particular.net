using System;
using NServiceBus;

public class OrderSagaData :
    IContainSagaData
{
    public Guid Id { get; set; }
    public string Originator { get; set; }
    public string OriginalMessageId { get; set; }
    public string OrderId { get; set; }
    public string PaymentTransactionId { get; set; }
}