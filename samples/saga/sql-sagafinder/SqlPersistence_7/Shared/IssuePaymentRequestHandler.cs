using System.Threading.Tasks;
using NServiceBus;

class IssuePaymentRequestHandler :
    IHandleMessages<IssuePaymentRequest>
{
    public Task Handle(IssuePaymentRequest message, IMessageHandlerContext context)
    {
        var completePayment = new CompletePaymentTransaction
        {
            PaymentTransactionId = message.PaymentTransactionId
        };
        return context.SendLocal(completePayment);
    }
}