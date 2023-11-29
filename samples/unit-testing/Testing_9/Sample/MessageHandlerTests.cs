using System.Threading.Tasks;
using NServiceBus.Testing;
using NUnit.Framework;

[TestFixture]
public class MessageHandlerTests
{
    #region HandlerTest
    [Test]
    public async Task ShouldReplyWithResponseMessage()
    {
        var handler = new MyReplyingHandler();
        var context = new TestableMessageHandlerContext();

        await handler.Handle(new MyRequest(), context)
            .ConfigureAwait(false);

        var repliedMessages = context.RepliedMessages;
        Assert.AreEqual(1, repliedMessages.Length);
        Assert.IsInstanceOf<MyResponse>(repliedMessages[0].Message);
    }
    #endregion
}