namespace Testing_6.AdditionalServices
{
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Testing;
    using NUnit.Framework;

    [Explicit]
    #region TestingAdditionalDependencies
    [TestFixture]
    public class Tests
    {
        [Test]
        public void RunWithDependencyInjection()
        {
            var mockService = new MyService();
            var injectionHandler = new WithDependencyInjectionHandler(mockService);
            var handler = Test.Handler(injectionHandler);
            // Rest of test
        }
    }

    class WithDependencyInjectionHandler :
        IHandleMessages<MyMessage>
    {
        MyService myService;

        public WithDependencyInjectionHandler(MyService myService)
        {
            this.myService = myService;
        }

        public Task Handle(MyMessage message, IMessageHandlerContext context)
        {
            // use myService here
            return Task.CompletedTask;
        }
    }

    #endregion

    class MyService
    {
    }

    class MyMessage
    {
    }
}