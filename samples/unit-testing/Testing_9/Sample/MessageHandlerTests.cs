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

        await handler.Handle(new MyRequest(), context);

        var repliedMessages = context.RepliedMessages;
        Assert.That(repliedMessages, Has.Length.EqualTo(1));
        Assert.That(repliedMessages[0].Message, Is.InstanceOf<MyResponse>());
    }
    #endregion
}