using Events;
using NServiceBus;

namespace Commands;

public class PlaceOrder : ICommand
{
    public Guid CustomerId { get; set; }
    public OrderDetails Order { get; set; } = null!;
}