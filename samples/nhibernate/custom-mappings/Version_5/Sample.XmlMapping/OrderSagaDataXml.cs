using System;
using NServiceBus.Saga;

public class OrderSagaDataXml : ContainSagaData
{
    public virtual string OrderId { get; set; }
    public virtual int Version { get; set; }
}