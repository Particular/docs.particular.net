using System.Threading.Tasks;
using NServiceBus.ApprovalTests;
using NServiceBus.Testing;
using NUnit.Framework;

[TestFixture]
public class MessageHandlerTests
{
    #region HandlerTest
    [Test]
    public async Task VerifyMyReplyingHandler()
    {
        var handler = new MyReplyingHandler();
        var context = new TestableMessageHandlerContext();

        await handler.Handle(new MyRequest(), context)
            .ConfigureAwait(false);

        TestContextVerifier.Verify(context);
    }
    #endregion
}