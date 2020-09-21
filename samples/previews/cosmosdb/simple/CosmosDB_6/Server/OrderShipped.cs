using System;

public class OrderShipped: IProvideOrderId
{
    public Guid OrderId { get; set; }
    public DateTime ShippingDate { get; set; }
}