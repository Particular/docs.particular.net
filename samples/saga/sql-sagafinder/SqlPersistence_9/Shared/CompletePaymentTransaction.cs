using NServiceBus;

public class CompletePaymentTransaction :
    IMessage
{
    public string PaymentTransactionId { get; set; }
}