using System;

public class PlaceOrderResponse :
    IMessage
{
    public Guid OrderId { get; set; }
    public string WorkerName { get; set; }
}