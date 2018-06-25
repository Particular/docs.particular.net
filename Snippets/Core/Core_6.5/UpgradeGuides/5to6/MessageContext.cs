namespace Core6.UpgradeGuides._5to6
{
    using System.Threading.Tasks;
    using Core6.Handlers;
    using NServiceBus;

    public class MessageContext :
        IHandleMessages<MyMessage>
    {
        #region 5to6-messagecontext

        public Task Handle(MyMessage message, IMessageHandlerContext context)
        {
            var messageId = context.MessageId;
            var replyToAddress = context.ReplyToAddress;
            var headers = context.MessageHeaders;
            return Task.CompletedTask;
        }

        #endregion
    }
}