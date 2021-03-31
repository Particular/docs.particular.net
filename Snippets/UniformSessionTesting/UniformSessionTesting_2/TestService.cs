using NServiceBus.UniformSession.Testing;

class TestService
{
    void TheTest()
    {
        #region UniformSessionServiceTesting
        var session = new TestableUniformSession();
        var someService = new SomeService(session);

        someService.DoTheThing();

        Assert.AreEqual(1, session.SentMessages.Length);
        #endregion
    }
}