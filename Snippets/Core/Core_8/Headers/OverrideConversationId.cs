using System.Threading.Tasks;
using Core8.Handlers;
using NServiceBus;

public class ConversationIdHeaderOverride
{
    public async Task PerMessage()
    {
        IMessageSession context = null;
        #region override-conversation-id
        var sendOptions = new SendOptions();
        sendOptions.SetHeader(Headers.ConversationId, "MyCustomConversationId/" + System.Guid.NewGuid());
        await context.Send(new MyMessage(), sendOptions)
            .ConfigureAwait(false);
        #endregion
    }

    public void GlobalConvention()
    {
        EndpointConfiguration config = null;
        #region custom-conversation-id-convention
        config.CustomConversationIdStrategy(context =>
        {
            if (context.Message.Instance is CancelOrder)
            {
                //use the order id as the conversation id
                return ConversationId.Custom("Order/" + ((CancelOrder)context.Message.Instance).OrderId);
            }

            //use the default generated id
            return ConversationId.Default;
        });
        #endregion
    }

    class CancelOrder
    {
        public string OrderId { get; set; }
    }
}
