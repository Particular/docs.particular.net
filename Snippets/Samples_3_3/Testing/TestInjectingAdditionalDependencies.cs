using NServiceBus;
using NServiceBus.Testing;
using NUnit.Framework;

namespace Snippets_3_3
{
    [TestFixture]
    class TestInjectingAdditionalDependencies
    {
        [Test]
        public void RunWithProperties()
        {
            Test.Initialize();

            var mockService = new MyService();
            Test.Handler<WithPropertyDependencyHandler>()
                .WithExternalDependencies(handler => handler.MyService = mockService);
            //Rest of test
        }

        class WithPropertyDependencyHandler : IHandleMessages<MyMessage>
        {
            public IBus Bus { get; set; }
            public MyService MyService { get; set; }

            public void Handle(MyMessage message)
            {
            }
        }

        class MyMessage : IMessage
        {
        }

        class MyService
        {
        }
    }
}