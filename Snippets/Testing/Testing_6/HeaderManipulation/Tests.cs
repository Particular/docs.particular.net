namespace Testing_6.HeaderManipulation
{
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Testing;
    using NUnit.Framework;

    [Explicit]
    #region TestingHeaderManipulation

    [TestFixture]
    public class Tests
    {
        [Test]
        public void Run()
        {
            Test.Handler<MyMessageHandler>()
                .SetIncomingHeader("MyHeaderKey", "myHeaderValue")
                .ExpectReply<ResponseMessage>((m, replyOptions) =>
                    replyOptions.GetHeaders()["MyHeaderKey"] == "myHeaderValue")
                .OnMessage<RequestMessage>(m => m.String = "hello");
        }
    }

    class MyMessageHandler :
        IHandleMessages<RequestMessage>
    {
        public Task Handle(RequestMessage message, IMessageHandlerContext context)
        {
            var header = context.MessageHeaders["MyHeaderKey"];

            var responseMessage = new ResponseMessage();

            var options = new ReplyOptions();
            options.SetHeader("MyHeaderKey", header);
            return context.Reply(responseMessage, options);
        }
    }

    #endregion
}
