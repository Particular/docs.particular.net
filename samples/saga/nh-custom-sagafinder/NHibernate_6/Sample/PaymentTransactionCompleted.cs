using NServiceBus;

public class PaymentTransactionCompleted :
    IEvent
{
    public string PaymentTransactionId { get; set; }
}