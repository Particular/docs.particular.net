using NServiceBus;

class IssuePaymentRequestHandler : IHandleMessages<IssuePaymentRequest>
{
    IBus bus;

    public IssuePaymentRequestHandler(IBus bus)
    {
        this.bus = bus;
    }

    public void Handle(IssuePaymentRequest message)
    {
        bus.Publish<PaymentTransactionCompleted>(evt =>
        {
            evt.PaymentTransactionId = message.PaymentTransactionId;
        });
    }
}