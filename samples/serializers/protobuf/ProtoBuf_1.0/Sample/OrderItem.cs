
using ProtoBuf;

[ProtoContract]
public class OrderItem
{
    [ProtoMember(1)]
    public int ItemId { get; set; }

    [ProtoMember(2)]
    public int Quantity { get; set; }
}