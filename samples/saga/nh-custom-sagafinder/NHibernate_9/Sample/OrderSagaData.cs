using NServiceBus;

public class OrderSagaData :
    ContainSagaData
{
    public virtual string OrderId { get; set; }
    public virtual string PaymentTransactionId { get; set; }
}