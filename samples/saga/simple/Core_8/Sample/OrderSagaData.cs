using NServiceBus;
using System;

public class OrderSagaData :
    ContainSagaData
{
    public Guid OrderId { get; set; }
}