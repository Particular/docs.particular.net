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
        var completePayment = new CompletePaymentTransaction
        {
            PaymentTransactionId = message.PaymentTransactionId
        };
        bus.SendLocal(completePayment);
    }
}