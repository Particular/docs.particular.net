using NServiceBus;
using NServiceBus.Testing;
using NUnit.Framework;

namespace Snippets_3_3
{
    [TestFixture]
    class TestConstructorInjection
    {
        [Test]
        public void RunWithConstructorDependency()
        {
            Test.Initialize();

            var mockService = new MyService();
            Test.Handler(bus => new WithConstructorDependencyHandler(mockService));
            //Rest of test
        }

        class WithConstructorDependencyHandler : IHandleMessages<MyMessage>
        {
            MyService myService;

            public WithConstructorDependencyHandler(MyService myService)
            {
                this.myService = myService;
            }

            public IBus Bus { get; set; }

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