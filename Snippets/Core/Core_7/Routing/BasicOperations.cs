namespace Core7.Routing
{
    using System.Threading.Tasks;
    using NServiceBus;

    class BasicOperations
    {
        async Task ConcreteMessage(IPipelineContext context)
        {
            #region InstancePublish

            var message = new MyEvent
            {
                SomeProperty = "Hello world"
            };
            await context.Publish(message)
                .ConfigureAwait(false);

            #endregion
        }

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