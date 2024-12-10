using System;
using NServiceBus;

public class OrderSagaData :
    ContainSagaData
{
    public Guid OrderId { get; set; }
}