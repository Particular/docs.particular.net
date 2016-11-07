using System.Threading.Tasks;
using NServiceBus;

class IssuePaymentRequestHandler :
    IHandleMessages<IssuePaymentRequest>
{
    public Task Handle(IssuePaymentRequest message, IMessageHandlerContext context)
    {
        var transactionCompleted = new PaymentTransactionCompleted
        {
            PaymentTransactionId = message.PaymentTransactionId
        };
        return context.Publish(transactionCompleted);
    }
}