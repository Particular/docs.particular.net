namespace Core8.Routing
{
    using System.Threading.Tasks;
    using NServiceBus;

    class BasicOperations
    {
        async Task InterfaceSend(IEndpointInstance endpoint)
        {
            #region InterfaceSend

            await endpoint.Send<IMyMessage>(message =>
            {
                message.SomeProperty = "Hello world";
            }).ConfigureAwait(false);

            #endregion
        }

        Task InterfaceReply(IMessageHandlerContext context)
        {
            #region InterfaceReply

            return context.Reply<IMyReply>(message =>
            {
                message.SomeProperty = "Hello world";
            });

            #endregion
        }

        Task InterfacePublish(IMessageHandlerContext context)
        {
            #region InterfacePublish

            return context.Publish<IMyEvent>(message =>
            {
                message.SomeProperty = "Hello world";
            });

            #endregion
        }

        class InterfaceMessageCreatedUpFront
        {
            IMessageHandlerContext context = null;
            IMessageSession messageSession = null;

            #region IMessageCreatorUsage

            //IMessageCreator is available via dependency injection
            async Task PublishEvent(IMessageCreator messageCreator)
            {
                var eventMessage = messageCreator.CreateInstance<IMyEvent>(message =>
                {
                    message.SomeProperty = "Hello world";
                });


                await messageSession.Publish(eventMessage);

                //or if on a message handler

                await context.Publish(eventMessage);
            }

            #endregion
        }

        async Task Subscribe(IEndpointInstance endpoint)
        {
            #region ExplicitSubscribe

            await endpoint.Subscribe<MyEvent>()
                .ConfigureAwait(false);

            await endpoint.Unsubscribe<MyEvent>()
                .ConfigureAwait(false);

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