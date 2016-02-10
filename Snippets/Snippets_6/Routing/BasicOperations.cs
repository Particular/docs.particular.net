namespace Snippets6.Routing
{
    using System.Threading.Tasks;
    using NServiceBus;

    public class BasicOperations
    {
        public async Task ConcreteMessage()
        {
            IPipelineContext context = null;
            #region InstancePublish
            MyEvent message = new MyEvent { SomeProperty = "Hello world" };
            await context.Publish(message);
            #endregion

        }

        public async Task InterfaceMessage()
        {
            IPipelineContext context = null;
            #region InterfacePublish
            await context.Publish<IMyEvent>(m => { m.SomeProperty = "Hello world"; });
            #endregion

        }

        public async Task Subscribe()
        {
            IEndpointInstance endpoint = null;
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