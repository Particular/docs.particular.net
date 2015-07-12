namespace Snippets6.Routing
{
    using NServiceBus;

    public class BasicOperations
    {
        public void ConcreteMessage()
        {
            IBus bus = null;
            #region InstancePublish
            var message = new MyEvent { SomeProperty = "Hello world" };

            bus.Publish(message);
            #endregion

        }

        public void InterfaceMessage()
        {
            IBus bus = null;
            #region InterfacePublish
            var message = new MyEvent { SomeProperty = "Hello world" };

            bus.Publish<IMyEvent>(m => { m.SomeProperty = "Hello world"; });
            #endregion

        }

        
        public void Subscribe()
        {
            IBus bus = null;
            #region ExplicitSubscribe
            bus.Subscribe<MyEvent>();

            bus.Unsubscribe<MyEvent>();
            #endregion

        }
        
        class MyEvent
        {
            public string SomeProperty { get; set; }
        }

        class IMyEvent
        {
            public string SomeProperty { get; set; }
        }

    }
}