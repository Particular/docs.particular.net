using Domain;

namespace Events;

public class OrderLine
{
    public Product Product { get; set; } = null!;
    public int Quantity { get; set; }
}