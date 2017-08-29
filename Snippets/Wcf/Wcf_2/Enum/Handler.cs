using System.Threading.Tasks;
using NServiceBus;

namespace Wcf1.Enum
{
    #region WcfEnumCallbackResponse

    public class Handler :
        IHandleMessages<Message>
    {
        public Task Handle(Message message, IMessageHandlerContext context)
        {
            return context.Reply(Status.OK);
        }
    }

    #endregion
}