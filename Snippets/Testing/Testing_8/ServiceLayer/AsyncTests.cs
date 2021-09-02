namespace Testing_8.ServiceLayer
{
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Testing;
    using NUnit.Framework;

    [Explicit]
    [TestFixture]
    public class AsyncTests
    {
        #region TestingServiceLayerAsync
        [Test]
        public async Task TestHandler()
        {
            await Test.Handler<MyAsyncHandler>()
                .ExpectReply<ResponseMessage>(
                    check: message =>
                    {
                        return message.String == "hello";
                    })
                .OnMessageAsync<RequestMessage>(
                    initializeMessage: message =>
                    {
                        message.String = "hello";
                    });
        }

    }

    public class MyAsyncHandler :
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
