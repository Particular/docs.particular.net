using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;
using NServiceBus.Transport.AzureServiceBus;

#region asb-configure-oversized-messages-handler
internal class CustomOversizedBrokeredMessageHandler : IHandleOversizedBrokeredMessages
{
    public Task Handle(BrokeredMessage brokeredMessage)
    {
        // do something usefull with the brokered message, e.g. store it in blob storage
        return Task.FromResult(true);
    }
}
#endregion