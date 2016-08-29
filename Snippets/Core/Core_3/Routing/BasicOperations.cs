namespace Core3.Routing
{
    using NServiceBus;

    class BasicOperations
    {
        void ConcreteMessage(IBus bus)
        {
            #region InstancePublish

            var message = new MyEvent
            {
                SomeProperty = "Hello world"
            };
            bus.Publish(message);

            #endregion
        }

        void InterfaceMessage(IBus bus)
        {
            #region InterfacePublish

            bus.Publish<IMyEvent>(
                messageConstructor: message =>
                {
                    message.SomeProperty = "Hello world";
                });

            #endregion
        }

        void Subscribe(IBus bus)
        {
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