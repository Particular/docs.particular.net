using NServiceBus.Testing;
using NServiceBus.UniformSession.Testing;

class TestHandler
{
    public void TheTest()
    {
        #region UniformSessionHandlerTesting
        var session = new TestableUniformSession();
        var sharedComponent = new SharedComponent(session);
        var handler = new SomeMessageHandler(sharedComponent);

        Test.Handler(handler)
            .WithUniformSession(session)
            .ExpectPublish<SomeEvent>()
            .OnMessage<SomeMessage>();
        #endregion
    }
}