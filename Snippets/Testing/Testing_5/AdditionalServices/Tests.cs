namespace Testing_5.AdditionalServices
{
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
            Test.Initialize();

            MyService mockService = new MyService();
            Test.Handler(bus => new WithDependencyInjectionHandler(mockService));
            //Rest of test
        }
    }

    class WithDependencyInjectionHandler : IHandleMessages<MyMessage>
    {
        MyService myService;

        public WithDependencyInjectionHandler(MyService myService)
        {
            this.myService = myService;
        }

        public void Handle(MyMessage message)
        {
        }
    }

    #endregion

    [Explicit]
    #region ConstructorInjectedBus
    [TestFixture]
    public class Tests2
    {
        [Test]
        public void RunWithConstructorInjectedBus()
        {
            Test.Initialize();

            MyService mockService = new MyService();
            Test.Handler(bus => new WithConstructorInjectedBusHandler(bus));
            //Rest of test
        }
    }

    class WithConstructorInjectedBusHandler : IHandleMessages<MyMessage>
    {
        IBus bus;

        public WithConstructorInjectedBusHandler(IBus bus)
        {
            this.bus = bus;
        }
        public void Handle(MyMessage message)
        {
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