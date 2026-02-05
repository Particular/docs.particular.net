using NServiceBus;

public record IssuePaymentRequest :
    IMessage
{
    public string PaymentTransactionId { get; init; }
}