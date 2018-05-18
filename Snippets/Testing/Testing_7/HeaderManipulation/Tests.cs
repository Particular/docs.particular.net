namespace Testing_7.HeaderManipulation
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
                .ExpectReply<ResponseMessage>(
                    check: (message, replyOptions) =>
                    {
                        var headers = replyOptions.GetHeaders();
                        return headers["MyHeaderKey"] == "myHeaderValue";
                    })
                .OnMessage<RequestMessage>(
                    initializeMessage: message =>
                    {
                        message.String = "hello";
                    });
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