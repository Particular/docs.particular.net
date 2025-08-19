namespace Testing_8.UpgradeGuides._7to8
{
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Testing;
    using NServiceBus.UniformSession;
    using NServiceBus.UniformSession.Testing;
    using NUnit.Framework;

    [Explicit]
    [TestFixture]
    class Tests
    {
        #region 7to8-uniformsession
        [Test]
        public async Task TestWithUniformSession()
        {
            var uniformSession = new TestableUniformSession();
            var component = new SharedComponent(uniformSession);

            await component.DoSomething();

            Assert.That(uniformSession.PublishedMessages, Has.Length.EqualTo(1));
        }
        #endregion

        #region 7to8-uniformsessioncontextwrapping
        [Test]
        public async Task TestWithMessageHandler()
        {
            var handlerContext = new TestableMessageHandlerContext();
            var uniformSession = new TestableUniformSession(handlerContext);
            var sharedComponent = new SharedComponent(uniformSession);
            var messageHandler = new SomeMessageHandler(sharedComponent);

            // message handler calls SharedComponent within Handle
            await messageHandler.Handle(new SomeEvent(), handlerContext);

            Assert.Multiple(() =>
            {
                Assert.That(uniformSession.SentMessages, Has.Length.EqualTo(1));
                // the message handler context and the uniform session share the same state, so these assertions are identical
                Assert.That(handlerContext.SentMessages, Has.Length.EqualTo(1));
            });
        }
        #endregion

        #region 7to8-uniformsessionmessagesessionwrapping
        [Test]
        public async Task TestWithMessageSession()
        {
            var messageSession = new TestableMessageSession();
            var uniformSession = new TestableUniformSession(messageSession);
            var sharedComponent = new SharedComponent(uniformSession);
            var myService = new MyService(sharedComponent);

            // MyService calls SharedComponent within Start
            await myService.Start(messageSession);

            Assert.Multiple(() =>
            {
                Assert.That(uniformSession.SentMessages, Has.Length.EqualTo(1));
                // the message session and the uniform session share the same state, so these assertions are identical
                Assert.That(messageSession.SentMessages, Has.Length.EqualTo(1));
            });
        }
        #endregion

        class SharedComponent
        {
            public SharedComponent(IUniformSession uniformSession)
            {
            }
            public Task DoSomething()
            {
                throw new System.NotImplementedException();
            }
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

        class SomeMessageHandler : IHandleMessages<SomeEvent>
        {
            public SomeMessageHandler(SharedComponent sharedComponent)
            {
            }

            public Task Handle(SomeEvent message, IMessageHandlerContext context)
            {
                throw new System.NotImplementedException();
            }
        }

        class SomeEvent : IEvent { }
        class SomeMessage : IMessage { }
    }
}