namespace Snippets3.Handlers
{
    using NServiceBus;

    public class ReplyingHandler : IHandleMessages<RequestDataMessage>
    {
        IBus Bus;

        public ReplyingHandler(IBus bus)
        {
            Bus = bus;
        }

        #region ReplyingMessageHandler

        public void Handle(RequestDataMessage message)
        {
            //Create a response message
            var response = new DataResponseMessage
            {
                DataId = message.DataId,
                String = message.String
            };

            //Underneath the covers, Reply sends a new message to the return address on the message being handled.
            Bus.Reply(response);

            //Reply is equivalent to the following code:
            Bus.Send(Bus.CurrentMessageContext.ReplyToAddress, Bus.CurrentMessageContext.Id, response);
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