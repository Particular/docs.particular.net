using NServiceBus;

public class PaymentTransactionCompleted :
    IMessage
{
    public string PaymentTransactionId { get; set; }
}