namespace Snippets6
{
    using System.Threading.Tasks;
    using NServiceBus;

    public class ReplyHandler : IHandleMessages<Request>
    {
        #region Replies-Basic

        public Task Handle(Request message, IMessageHandlerContext context)
        {
            //Create a response message
            var response = new Response
            {
                Value = CalculateBasedOn(message.Data)
            };

            return context.Reply(response);
        }

        #endregion

        string CalculateBasedOn(string data)
        {
            return data;
        }
    }

    public class ReplyViaSendHandler : IHandleMessages<Request>
    {
        public Task Handle(Request message, IMessageHandlerContext context)
        {
            #region Replies-ViaSend

            SendOptions sendOptions = new SendOptions();
            sendOptions.SetDestination(context.ReplyToAddress);
            sendOptions.SetCorrelationId(context.MessageId);
            return context.Send(new Response(), sendOptions);

            #endregion
        }
    }

    public class Request : IMessage
    {
        public string Data { get; set; }
    }

    public class Response : IMessage
    {
        public string Value { get; set; }
    }
}