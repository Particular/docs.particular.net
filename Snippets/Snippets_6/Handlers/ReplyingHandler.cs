namespace Snippets6.Handlers
{
    using System.Threading.Tasks;
    using NServiceBus;

    public class ReplyingHandler : IHandleMessages<RequestDataMessage>
    {
        #region ReplyingMessageHandler

        public async Task Handle(RequestDataMessage message, IMessageHandlerContext context)
        {
            //Create a response message:
            var response = new DataResponseMessage
            {
                DataId = message.DataId,
                String = message.String
            };

            //Underneath the covers, Reply sends a new message to the return address on the message being handled.
            await context.Reply(response);
        }

        #endregion ReplyingMessageHandler

    }

    public class RequestDataMessage : ICommand
    {
        public string DataId { get; set; }
        public string String { get; set; }
    }

    public class DataResponseMessage
    {
        public string DataId { get; set; }
        public string String { get; set; }
    }
}