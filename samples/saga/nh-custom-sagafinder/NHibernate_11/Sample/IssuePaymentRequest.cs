record IssuePaymentRequest : IMessage
{
    public required string PaymentTransactionId { get; init; }
}