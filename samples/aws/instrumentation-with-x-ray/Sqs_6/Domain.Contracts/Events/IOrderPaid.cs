namespace Events;

public interface IOrderPaid : IEvent
{
    Guid CustomerId { get; set; }
    OrderDetails Order { get; set; }
}