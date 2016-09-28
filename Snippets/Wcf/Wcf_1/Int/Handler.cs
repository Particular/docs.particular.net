using System.Threading.Tasks;
using NServiceBus;

namespace Wcf1.Int
{
    #region WcfIntCallbackResponse

    public class Handler :
        IHandleMessages<Message>
    {
        public Task Handle(Message message, IMessageHandlerContext context)
        {
            return context.Reply(10);
        }
    }

    #endregion
}