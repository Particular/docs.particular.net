using System;
using NServiceBus.Saga;
using NHibernate.Mapping.Attributes;
using NHibernate.SqlTypes;
using NHibernate.Type;

[Class]
public class OrderSagaDataAttributes : IContainSagaData
{
    [Id(Name = "Id")]
    public virtual Guid Id { get; set; }
    [Property]
    public virtual string OriginalMessageId { get; set; }
    [Property]
    public virtual string Originator { get; set; }
    [Property(Length = 100, Type = "AnsiString", Unique = true)]
    public virtual string OrderId { get; set; }
    [Version]
    public virtual int Version { get; set; }
}

