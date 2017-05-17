
using ProtoBuf;

[ProtoContract]
public class OrderItem
{
    [ProtoMember(1)]
    public int ItemId;

    [ProtoMember(2)]
    public int Quantity;
}