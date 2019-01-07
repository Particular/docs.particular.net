using System.Threading.Tasks;
using NServiceBus.ApprovalTests;
using NServiceBus.Testing;
using NUnit.Framework;

[TestFixture]
public class MessageHandlerTests
{
    #region HandlerTest
    [Test]
    public async Task VerifyHandlerResult()
    {
        var handler = new MyHandler();
        var context = new TestableMessageHandlerContext();

        await handler.Handle(new MyRequest(), context)
            .ConfigureAwait(false);

        TestContextVerifier.Verify(context);
    }
    #endregion
}