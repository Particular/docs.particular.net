namespace Core5.Handlers
{
    using NServiceBus;

    public class ReplyingHandler :
        IHandleMessages<RequestDataMessage>
    {
        IBus bus;

        public ReplyingHandler(IBus bus)
        {
            this.bus = bus;
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
            bus.Reply(response);

            //Reply is equivalent to the following code:
            bus.Send(bus.CurrentMessageContext.ReplyToAddress, bus.CurrentMessageContext.Id, response);
        }

        #endregion ReplyingMessageHandler

    }

    public class RequestDataMessage :
        ICommand
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
