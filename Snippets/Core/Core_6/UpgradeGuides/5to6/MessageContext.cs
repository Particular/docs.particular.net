namespace Core6.UpgradeGuides._5to6
{
    using System.Threading.Tasks;
    using NServiceBus;
    using Handlers;

    public class MessageContext : IHandleMessages<MyMessage>
    {
        #region 5to6-messagecontext

        public async Task Handle(MyMessage message, IMessageHandlerContext context)
        {
            var messageId = context.MessageId;
            var replyToAddress = context.ReplyToAddress;
            var headers = context.MessageHeaders;
        }

        #endregion
    }
}