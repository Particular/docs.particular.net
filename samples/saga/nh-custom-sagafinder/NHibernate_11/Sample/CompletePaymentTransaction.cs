public record CompletePaymentTransaction : IMessage
{
    public required string PaymentTransactionId { get; init; }
}