using System.Threading.Tasks;
using NServiceBus.Testing;
using NUnit.Framework;

[Explicit]
[TestFixture]
public class MessageHandlerTests
{
    #region HandlerTest
    [Test]
    public async Task ShouldReplyWithResponseMessage()
    {
        var handler = new MyReplyingHandler();
        var context = new TestableMessageHandlerContext();

        await handler.Handle(new MyRequest(), context);

        Assert.That(context.RepliedMessages, Has.Length.EqualTo(1));
        Assert.That(context.RepliedMessages[0].Message, Is.InstanceOf<MyResponse>());
    }
    #endregion
}