namespace Testing_8.HeaderManipulation
{
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Testing;
    using NUnit.Framework;

    [Explicit]
    #region TestingHeaderManipulationAsync
    [TestFixture]
    public class AsyncTests
    {

        [Test]
        public async Task Run()
        {
            await Test.Handler<MyAsyncMessageHandler>()
                .SetIncomingHeader("MyHeaderKey", "myHeaderValue")
                .ExpectReply<ResponseMessage>(
                    check: (message, replyOptions) =>
                    {
                        var headers = replyOptions.GetHeaders();
                        return headers["MyHeaderKey"] == "myHeaderValue";
                    })
                .OnMessageAsync<RequestMessage>(
                    initializeMessage: message =>
                    {
                        message.String = "hello";
                    });
        }
    }

    class MyAsyncMessageHandler :
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
