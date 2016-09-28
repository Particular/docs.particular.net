namespace Wcf1.Object
{
    using System.Threading.Tasks;
    using NServiceBus;

    #region WcfObjectCallbackResponse

    public class Handler :
        IHandleMessages<Message>
    {
        public Task Handle(Message message, IMessageHandlerContext context)
        {
            var responseMessage = new ResponseMessage
            {
                Property = "PropertyValue"
            };
            return context.Reply(responseMessage);
        }
    }

    #endregion
}