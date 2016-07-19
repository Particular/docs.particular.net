using NServiceBus;

#region MessageContract
public class PlaceOrder :
    IMessage
{
    public string OrderId { get; set; }
}
#endregion