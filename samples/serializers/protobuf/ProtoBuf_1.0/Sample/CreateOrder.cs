using System;
using System.Collections.Generic;
using NServiceBus;
using ProtoBuf;

#region message
[ProtoContract]
public class CreateOrder :
    IMessage
{
    [ProtoMember(1)]
    public int OrderId { get; set; }

    [ProtoMember(2)]
    public DateTime Date { get; set; }

    [ProtoMember(3)]
    public int CustomerId { get; set; }

    [ProtoMember(4)]
    public List<OrderItem> OrderItems { get; set; }
}
#endregion