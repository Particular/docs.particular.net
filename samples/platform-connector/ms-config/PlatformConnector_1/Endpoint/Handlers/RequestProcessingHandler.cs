using System.Threading.Tasks;
using NServiceBus;

class RequestProcessingHandler : IHandleMessages<RequestProcessing>
{
    public async Task Handle(RequestProcessing message, IMessageHandlerContext context)
    {
        await Task.Delay(500);
        await context.Reply(new RequestProcessingResponse {BusinessId = message.BusinessId});
    }
}