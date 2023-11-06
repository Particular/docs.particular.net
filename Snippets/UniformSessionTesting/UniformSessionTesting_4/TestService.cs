using NServiceBus.UniformSession.Testing;

class TestService
{
    void TheTest()
    {
        #region UniformSessionServiceTesting
        var session = new TestableUniformSession();
        var service = new SomeService(session);

        service.DoTheThing();

        Assert.AreEqual(1, session.SentMessages.Length);
        #endregion
    }
}