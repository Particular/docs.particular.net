using NServiceBus;

public class OrderSagaData :
    ContainSagaData
{
    public string OrderId { get; set; }
    public string PaymentTransactionId { get; set; }
}