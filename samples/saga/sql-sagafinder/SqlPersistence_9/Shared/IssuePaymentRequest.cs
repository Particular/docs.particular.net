using NServiceBus;

record IssuePaymentRequest :
    IMessage
{
    public string PaymentTransactionId { get; init; }
}