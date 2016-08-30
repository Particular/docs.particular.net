using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;
using NServiceBus.Transport.AzureServiceBus;

#region asb-configure-oversized-messages-handler
class CustomOversizedBrokeredMessageHandler :
    IHandleOversizedBrokeredMessages
{
    public Task Handle(BrokeredMessage brokeredMessage)
    {
        // do something useful with the brokered message
        // e.g. store it in blob storage
        return Task.FromResult(true);
    }
}
#endregion