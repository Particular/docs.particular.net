using NServiceBus.Testing;
using NServiceBus.UniformSession.Testing;

public class MessageSessionTesting
{
    public void TheTest()
    {
        #region UniformSessionMessageSessionTesting

        var messageSession = new TestableMessageSession();
        // pass the message session to the TestableUniformSession:
        var uniformSession = new TestableUniformSession(messageSession);
        var sharedComponent = new SharedComponent(uniformSession);
        var myService = new MyService(sharedComponent);

        myService.Start(messageSession);

        Assert.AreEqual(1, messageSession.PublishedMessages.Length);

        #endregion
    }
}