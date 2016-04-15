namespace Snippets6.Callback.Object
{
    using System.Threading.Tasks;
    using NServiceBus;

    #region ObjectCallbackResponse

    public class Handler : IHandleMessages<Message>
    {
        public async Task Handle(Message message, IMessageHandlerContext context)
        {
            await context.Reply(new ResponseMessage
            {
                Property = "PropertyValue"
            });
        }
    }

    #endregion
}

