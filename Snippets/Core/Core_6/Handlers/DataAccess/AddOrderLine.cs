namespace Core6.Handlers.DataAccess
{
    public class AddOrderLine :
        OrderMessage
    {
        public object Product { get; set; }
        public object Quantity { get; set; }
        public object LineId { get; set; }
    }
}