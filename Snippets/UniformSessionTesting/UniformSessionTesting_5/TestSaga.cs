using NServiceBus.Testing;
using NServiceBus.UniformSession.Testing;

class TestSaga
{
    public void TheTest()
    {
        #region UniformSessionSagaTesting
        var handlerContext = new TestableMessageHandlerContext();
        var uniformSession = new TestableUniformSession(handlerContext);
        var saga = new SomeSaga(uniformSession);

        saga.Handle(new SomeMessage(), handlerContext);

        Assert.AreEqual(1, uniformSession.PublishedMessages.Length);
        #endregion
    }
}