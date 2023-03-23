namespace Core7.Conversation
{
    using System.Threading.Tasks;
    using NServiceBus;

    public class NewConversationIdStarter
    {
        public async Task NewConversationIdWithDefault()
        {
            IMessageHandlerContext context = null;
            #region new-conversation-id
            var sendOptions = new SendOptions();
            sendOptions.StartNewConversation();
            
            await context.Send(new CancelOrder(), sendOptions);
            
            #endregion
        }

        public async Task NewConversationIdWithCustomValue()
        {
            IMessageHandlerContext context = null;
            #region new-conversation-custom-id
            var sendOptions = new SendOptions();
            sendOptions.StartNewConversation("MyCustomConversationId/" + System.Guid.NewGuid());
            
            await context.Send(new CancelOrder(), sendOptions);
            
            #endregion
        }

        class CancelOrder
        {
            public string OrderId { get; set; }
        }
    }

}