using Events;
using NServiceBus;

namespace Commands;

public class ChargeOrder : ICommand
{
    public Guid CustomerId { get; set; }
    public OrderDetails Order { get; set; } = null!;
}