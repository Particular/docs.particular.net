namespace Core5.Handlers
{
    public class AddOrderLine : OrderMessage
    {
        public object Product { get; set; }
        public object Quantity { get; set; }
        public object LineId { get; set; }
    }
}