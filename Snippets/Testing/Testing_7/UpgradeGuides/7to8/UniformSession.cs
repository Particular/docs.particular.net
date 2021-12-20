namespace Testing_7.UpgradeGuides._7to8
{
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Testing;
    using NServiceBus.UniformSession;
    using NServiceBus.UniformSession.Testing;
    using NUnit.Framework;

    class Tests
    {
        #region 7to8-uniformsessioncontextwrapping
        [Test]
        public void TestWithMessageHandler()
        {
            var uniformSession = new TestableUniformSession();
            var sharedComponent = new SharedComponent(uniformSession);
            var handler = new SomeMessageHandler(sharedComponent);

            Test.Handler(handler)
                .WithUniformSession(uniformSession)
                .ExpectPublish<SomeEvent>()
                .OnMessage<SomeMessage>();
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