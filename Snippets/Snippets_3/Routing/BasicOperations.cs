namespace Snippets3.Routing
{
    using NServiceBus;

    public class BasicOperations
    {
        public void ConcreteMessage()
        {
            IBus bus = null;
            #region InstancePublish
            MyEvent message = new MyEvent { SomeProperty = "Hello world" };
            bus.Publish(message);
            #endregion

        }

        public void InterfaceMessage()
        {
            IBus bus = null;
            #region InterfacePublish
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

        interface IMyEvent
        {
            string SomeProperty { get; set; }
        }

    }
}