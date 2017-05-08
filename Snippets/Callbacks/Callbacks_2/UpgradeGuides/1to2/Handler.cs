namespace Callbacks.UpgradeGuides._1to2
{
    using System.Threading.Tasks;
    using NServiceBus;

    #region 1to2-Callbacks-ObjectCallbackResponse

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

    public class Message :
        IMessage
    {
    }

    public class ResponseMessage :
        IMessage
    {
        public string Property { get; set; }
    }
}