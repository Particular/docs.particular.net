using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Testing;
using NServiceBus.UniformSession.Testing;

namespace UniformSessionTesting_3
{
    class Tests
    {
        public async Task TestComponent()
        {
            #region 7to8-uniformsession

            var uniformSession = new TestableUniformSession();
            var component = new SharedComponent(uniformSession);

            await component.DoSomething();

            Assert.AreEqual(1, uniformSession.SentMessages.Length);

            #endregion
        }

        public void TestUniformSessionAndContext()
        {
            #region 7to8-uniformsessioncontextwrapping

            var uniformSession = new TestableUniformSession();
            var sharedComponent = new SharedComponent(uniformSession);
            var handler = new SomeMessageHandler(sharedComponent);

            Test.Handler(handler)
                .WithUniformSession(uniformSession)
                .ExpectPublish<SomeEvent>()
                .OnMessage<SomeMessage>();

            #endregion
        }
    }
}