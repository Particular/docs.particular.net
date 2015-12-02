namespace Snippets4
{
    using NServiceBus;

    public class ReplyHandler : IHandleMessages<Request>
    {
        #region Replies-Basic

        public IBus Bus { get; set; }

        public void Handle(Request message)
        {
            //Create a response message
            var response = new Response
            {
                Value = CalculateBasedOn(message.Data)
            };

            Bus.Reply(response);
        }
        
        #endregion

        string CalculateBasedOn(string data)
        {
            return data;
        }
    }

    public class ReplyViaSendHandler : IHandleMessages<Request>
    {
        public IBus Bus { get; set; }

        public void Handle(Request message)
        {
            #region Replies-ViaSend
            Bus.Send(Bus.CurrentMessageContext.ReplyToAddress, Bus.CurrentMessageContext.Id, new Response());
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