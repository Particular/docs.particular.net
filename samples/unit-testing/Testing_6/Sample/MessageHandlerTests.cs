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

        Assert.AreEqual(1, context.RepliedMessages.Length);
        Assert.IsInstanceOf<MyResponse>(context.RepliedMessages[0].Message);
    }
    #endregion
}