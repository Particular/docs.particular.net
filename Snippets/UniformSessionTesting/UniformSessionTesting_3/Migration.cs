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

            Assert.AreEqual(1, uniformSession.PublishedMessages.Length);

            #endregion
        }

        public async Task TestUniformSessionAndContext()
        {
            #region 7to8-uniformsessioncontextwrapping

            var handlerContext = new TestableMessageHandlerContext();
            var uniformSession = new TestableUniformSession(handlerContext);
            var sharedComponent = new SharedComponent(uniformSession);
            var messageHandler = new SomeMessageHandler(sharedComponent);

            // message handler calls SharedComponent within Handle
            await messageHandler.Handle(new SomeEvent(), handlerContext);

            Assert.AreEqual(1, uniformSession.SentMessages.Length);
            // the message handler context and the uniform session share the same state, so these assertions are identical
            Assert.AreEqual(1, handlerContext.SentMessages.Length);

            #endregion
        }

        public async Task TestUniformSessionAndMessageSession()
        {
            #region 7to8-uniformsessionmessagesessionwrapping

            var messageSession = new TestableMessageSession();
            var uniformSession = new TestableUniformSession(messageSession);
            var sharedComponent = new SharedComponent(uniformSession);
            var myService = new MyService(sharedComponent);

            // MyService calls SharedComponent within Start
            await myService.Start(messageSession);

            Assert.AreEqual(1, uniformSession.SentMessages.Length);
            // the message session and the uniform session share the same state, so these assertions are identical
            Assert.AreEqual(1, messageSession.SentMessages.Length);

            #endregion
        }

        class MyService
        {
            public MyService(SharedComponent sharedComponent)
            {
            }

            public Task Start(IMessageSession session)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}