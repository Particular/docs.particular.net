namespace Snippets6.UnitTesting.HeaderManipulation
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

    class MyMessageHandler : IHandleMessages<RequestMessage>
    {
        public async Task Handle(RequestMessage message, IMessageHandlerContext context)
        {
            string header = context.MessageHeaders["MyHeaderKey"];

            ResponseMessage responseMessage = new ResponseMessage();

            ReplyOptions options = new ReplyOptions();
            options.SetHeader("MyHeaderKey", header);
            await context.Reply(responseMessage, options);
        }
    }

    #endregion
}
