namespace Testing_6.AdditionalServices
{
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Testing;
    using NUnit.Framework;

    // ReSharper disable SuggestVarOrType_Elsewhere

    [Explicit]
    #region TestingAdditionalDependencies
    [TestFixture]
    public class Tests
    {
        [Test]
        public void RunWithDependencyInjection()
        {
            var mockService = new MyService();
            var handler = Test.Handler(new WithDependencyInjectionHandler(mockService));
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
            return Task.FromResult(0);
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