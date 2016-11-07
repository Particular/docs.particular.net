using NServiceBus;

class IssuePaymentRequestHandler :
    IHandleMessages<IssuePaymentRequest>
{
    IBus bus;

    public IssuePaymentRequestHandler(IBus bus)
    {
        this.bus = bus;
    }

    public void Handle(IssuePaymentRequest message)
    {
        var transactionCompleted = new PaymentTransactionCompleted
        {
            PaymentTransactionId = message.PaymentTransactionId
        };
        bus.Publish(transactionCompleted);
    }
}