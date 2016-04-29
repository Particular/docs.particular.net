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
        public void Run()
        {
            Test.Handler<MyHandler>()
                .ExpectReply<ResponseMessage>(m => m.String == "hello")
                .OnMessage<RequestMessage>(m => m.String = "hello");
        }
    }

    public class MyHandler :
        IHandleMessages<RequestMessage>
    {
        public async Task Handle(RequestMessage message, IMessageHandlerContext context)
        {
            ResponseMessage reply = new ResponseMessage
            {
                String = message.String
            };
            await context.Reply(reply);
        }
    }

    #endregion
}