using NServiceBus.UniformSession.Testing;

class TestService
{
    void TheTest()
    {
        #region UniformSessionServiceTesting
        var uniformSession = new TestableUniformSession();
        var someService = new SomeService(uniformSession);

        someService.DoTheThing();

        Assert.AreEqual(1, uniformSession.SentMessages.Length);
        #endregion
    }
}