namespace Core6.Routing
{
    using System.Threading.Tasks;
    using NServiceBus;

    class BasicOperations
    {
        Task InterfaceMessage(IPipelineContext context)
        {
            #region InterfacePublish

            return context.Publish<IMyEvent>(
                messageConstructor: message =>
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
            public string SomeProperty { get; set; }
        }

        interface IMyEvent
        {
            string SomeProperty { get; set; }
        }

    }
}