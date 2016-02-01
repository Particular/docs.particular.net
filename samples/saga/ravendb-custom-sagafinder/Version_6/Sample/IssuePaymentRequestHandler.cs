using System.Threading.Tasks;
using NServiceBus;

class IssuePaymentRequestHandler : IHandleMessages<IssuePaymentRequest>
{
    public Task Handle(IssuePaymentRequest message, IMessageHandlerContext context)
    {
        return context.Publish<PaymentTransactionCompleted>(evt =>
        {
            evt.PaymentTransactionId = message.PaymentTransactionId;
        });
    }
}