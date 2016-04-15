namespace Core6.Routing
{
    using System.Threading.Tasks;
    using NServiceBus;

    class BasicOperations
    {
        async Task ConcreteMessage(IPipelineContext context)
        {
            #region InstancePublish
            MyEvent message = new MyEvent { SomeProperty = "Hello world" };
            await context.Publish(message);
            #endregion
        }

        async Task InterfaceMessage(IPipelineContext context)
        {
            #region InterfacePublish
            await context.Publish<IMyEvent>(m => { m.SomeProperty = "Hello world"; });
            #endregion
        }

        async Task Subscribe(IEndpointInstance endpoint)
        {
            #region ExplicitSubscribe
            await endpoint.Subscribe<MyEvent>();

            await endpoint.Unsubscribe<MyEvent>();
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