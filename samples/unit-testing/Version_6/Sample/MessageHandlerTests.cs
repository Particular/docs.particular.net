using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Testing;
using NUnit.Framework;

namespace Sample
{
    [TestFixture]
    public class MessageHandlerTests
    {
        #region HandlerTest
        [Test]
        public async Task ShouldReplyWithResponseMessage()
        {
            var handler = new MyHandler();
            var context = new TestableMessageHandlerContext();

            await handler.Handle(new MyRequest(), context);

            Assert.IsTrue(context.RepliedMessages.Length == 1);
            Assert.IsInstanceOf<MyResponse>(context.RepliedMessages[0].Message);
        }
        #endregion
    }

    public class MyRequest
    {
    }

    public class MyResponse
    {
    }

    #region SimpleHandler
    public class MyHandler : IHandleMessages<MyRequest>
    {
        public Task Handle(MyRequest message, IMessageHandlerContext context)
        {
            return context.Reply(new MyResponse());
        }
    }
    #endregion
}
