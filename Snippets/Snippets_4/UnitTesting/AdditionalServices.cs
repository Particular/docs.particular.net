using NServiceBus;
using NServiceBus.Testing;
using NUnit.Framework;

namespace UnitTesting.AdditionalDependencies
{

    #region TestingAdditionalDependencies

    public class MyTest
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

    class MyMessage
    {
    }

    class MyService
    {
    }
}