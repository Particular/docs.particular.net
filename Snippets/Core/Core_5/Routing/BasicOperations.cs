namespace Core5.Routing
{
    using NServiceBus;

    class BasicOperations
    {
        void InterfaceSend(IBus bus)
        {
            #region InterfaceSend

            bus.Send<IMyMessage>(message =>
            {
                message.SomeProperty = "Hello world";
            });

            #endregion
        }

        void InterfaceReply(IBus bus)
        {
            #region InterfaceReply

            bus.Reply<IMyReply>(message =>
            {
                message.SomeProperty = "Hello world";
            });

            #endregion
        }

        void InterfacePublish(IBus bus)
        {
            #region InterfacePublish

            bus.Publish<IMyEvent>(message =>
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
        }

        interface IMyMessage
        {
            string SomeProperty { get; set; }
        }

        interface IMyReply
        {
            string SomeProperty { get; set; }
        }

        interface IMyEvent
        {
            string SomeProperty { get; set; }
        }
    }
}