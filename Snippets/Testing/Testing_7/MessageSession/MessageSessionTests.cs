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

        var someThingThatUseTheMessageSession = new SomeThingThatUseTheMessageSession(testableSession);

        await someThingThatUseTheMessageSession.DoSomething();

        Assert.AreEqual(1, testableSession.SentMessages.Length);
        Assert.IsInstanceOf<MyResponse>(testableSession.SentMessages[0].Message);
    }
    #endregion

    class SomeThingThatUseTheMessageSession
    {
        IMessageSession session;

        public SomeThingThatUseTheMessageSession(IMessageSession session)
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