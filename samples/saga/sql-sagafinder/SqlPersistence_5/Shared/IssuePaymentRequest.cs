using NServiceBus;

class IssuePaymentRequest :
    IMessage
{
    public string PaymentTransactionId { get; set; }
}