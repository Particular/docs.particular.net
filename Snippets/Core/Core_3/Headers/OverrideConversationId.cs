using NServiceBus;

public class ConversationIdHeaderOverride
{
    public void Handle()
    {
        IBus Bus = null;
        #region override-conversation-id
        Bus.OutgoingHeaders["NServiceBus.ConversationId"] = "MyCustomConversationId/" + System.Guid.NewGuid();
        #endregion
    }
}
