using System;
using NServiceBus.Saga;

public class OrderSagaData : IContainSagaData
{
    public Guid Id { get; set; }
    public string Originator { get; set; }
    public string OriginalMessageId { get; set; }
    public string OrderId { get; set; }
}