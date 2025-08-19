using NServiceBus;

public record CompletePaymentTransaction :
    IMessage
{
    public string PaymentTransactionId { get; init; }
}