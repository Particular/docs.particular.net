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
        var paymentTransactionCompleted = new PaymentTransactionCompleted
        {
            PaymentTransactionId = message.PaymentTransactionId
        };
        bus.Publish(paymentTransactionCompleted);
    }
}
