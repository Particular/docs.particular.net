namespace Core6.UpgradeGuides._5to6
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using NServiceBus;
    using Handlers;

    public class MessageContext : IHandleMessages<MyMessage>
    {
        #region 5to6-messagecontext

        public async Task Handle(MyMessage message, IMessageHandlerContext context)
        {
            string messageId = context.MessageId;
            string replyToAddress = context.ReplyToAddress;
            IReadOnlyDictionary<string, string> headers = context.MessageHeaders;
        }

        #endregion
    }
}