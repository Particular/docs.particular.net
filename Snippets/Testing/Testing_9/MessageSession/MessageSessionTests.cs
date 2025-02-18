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

        Assert.That(testableSession.SentMessages, Has.Length.EqualTo(1));
        Assert.That(testableSession.SentMessages[0].Message, Is.InstanceOf<MyResponse>());
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