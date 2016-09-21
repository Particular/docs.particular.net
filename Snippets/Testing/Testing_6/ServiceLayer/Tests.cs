namespace Testing_6.ServiceLayer
{
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Testing;
    using NUnit.Framework;

    [Explicit]
    #region TestingServiceLayer
    [TestFixture]
    public class Tests
    {
        [Test]
        public void TestHandler()
        {
            Test.Handler<MyHandler>()
                .ExpectReply<ResponseMessage>(m => m.String == "hello")
                .OnMessage<RequestMessage>(m => m.String = "hello");
        }

        [Test]
        public async Task TestHandler_AAA()
        {
            var handler = new MyHandler();
            var context = new TestableMessageHandlerContext();

            await handler.Handle(new RequestMessage {String = "hello"}, context)
                .ConfigureAwait(false);

            Assert.AreEqual(1, context.RepliedMessages.Length);
            Assert.IsInstanceOf<ResponseMessage>(context.RepliedMessages[0].Message);
            Assert.AreEqual("hello", ((ResponseMessage)context.RepliedMessages[0].Message).String);
        }
    }

    public class MyHandler :
        IHandleMessages<RequestMessage>
    {
        public Task Handle(RequestMessage message, IMessageHandlerContext context)
        {
            var reply = new ResponseMessage
            {
                String = message.String
            };
            return context.Reply(reply);
        }
    }

    #endregion
}