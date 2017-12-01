using System.Threading.Tasks;
using Core7.Handlers;
using NServiceBus;

public class ConversationIdHeaderOverride
{
    public async Task Handle()
    {
        IMessageSession context = null;
        #region override-conversation-id
        var sendOptions = new SendOptions();
        sendOptions.SetHeader(Headers.ConversationId, "MyCustomConversationId/" + System.Guid.NewGuid());
        await context.Send(new MyMessage(), sendOptions)
            .ConfigureAwait(false);
        #endregion
    }
}
