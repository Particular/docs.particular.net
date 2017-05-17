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
    public int OrderId;

    [ProtoMember(2)]
    public DateTime Date;

    [ProtoMember(3)]
    public int CustomerId;

    [ProtoMember(4)]
    public List<OrderItem> OrderItems;
}
#endregion