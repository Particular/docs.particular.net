using NServiceBus.Testing;
using NServiceBus.UniformSession.Testing;

class TestHandler
{
    void TheTest()
    {
        #region UniformSessionHandlerTesting
        var session = new TestableUniformSession();
        var handler = new SomeMessageHandler(session);

        Test.Handler(handler)
            .WithUniformSession(session)
            .ExpectPublish<SomeEvent>()
            .OnMessage<SomeMessage>();
        #endregion
    }
}