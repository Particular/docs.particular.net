public class OrderSagaData : ContainSagaData
{
    public string? OrderId { get; set; }

    public bool CustomerBilled { get; set; }

    public bool InventoryStaged { get; set; }
}