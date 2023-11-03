using Domain;
using NServiceBus;

namespace Commands;

public class UpdateProductStock : ICommand
{
    public bool AddStock { get; set; }
    public int Quantity { get; set; }
    public string ProductId { get; set; } = null!;
    public StockOperation StockOperation { get; set; }
}