namespace Core5.Routing
{
    using System.Threading.Tasks;
    using NServiceBus;

    class BasicOperations
    {
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

        class InterfaceMessageCreatedUpFront
        {
            IBus bus = null;

            #region IMessageCreatorUsage

            //IMessageCreator is available via dependency injection
            void PublishEvent(IMessageCreator messageCreator)
            {
                var eventMessage = messageCreator.CreateInstance<IMyEvent>(message =>
                {
                    message.SomeProperty = "Hello world";
                });


                bus.Publish(eventMessage);
            }

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