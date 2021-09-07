using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Testing;
using NUnit.Framework;

[TestFixture]
public class MessageSessionTests
{
    #region TestMessageSessionSend
    [Test]
    public async Task ShouldLogCorrectly()
    {
        var testableSession = new TestableMessageSession();

        var somethingThatUsesTheMessageSession = new SomethingThatUsesTheMessageSession(testableSession);

        await somethingThatUsesTheMessageSession.DoSomething();

        Assert.AreEqual(1, testableSession.SentMessages.Length);
        Assert.IsInstanceOf<MyResponse>(testableSession.SentMessages[0].Message);
    }
    #endregion

    class SomethingThatUsesTheMessageSession
    {
        IMessageSession session;

        public SomethingThatUsesTheMessageSession(IMessageSession session)
        {
            this.session = session;
        }

        public Task DoSomething()
        {
            return session.Send(new MyMessage());
        }
    }
    class MyMessage : IMessage
    {
    }
}