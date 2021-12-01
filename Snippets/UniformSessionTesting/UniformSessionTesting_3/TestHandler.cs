using System.Threading.Tasks;
using NServiceBus.Testing;
using NServiceBus.UniformSession.Testing;

class TestHandler
{
    async Task TheTest()
    {
        #region UniformSessionHandlerTesting
        var handlerContext = new TestableMessageHandlerContext();
        var uniformSession = new TestableUniformSession(handlerContext);
        var handler = new SomeMessageHandler(new SharedComponent(uniformSession));

        await handler.Handle(new SomeEvent(), handlerContext);

        Assert.AreEqual(1, uniformSession.SentMessages.Length);
        #endregion
    }
}