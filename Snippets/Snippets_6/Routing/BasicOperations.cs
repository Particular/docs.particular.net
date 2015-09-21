namespace Snippets6.Routing
{
    using System.Threading.Tasks;
    using NServiceBus;

    public class BasicOperations
    {
        public async Task ConcreteMessage()
        {
            IBus bus = null;
            #region InstancePublish
            var message = new MyEvent { SomeProperty = "Hello world" };
            await bus.PublishAsync(message);
            #endregion

        }

        public async Task InterfaceMessage()
        {
            IBus bus = null;
            #region InterfacePublish
            await bus.PublishAsync<IMyEvent>(m => { m.SomeProperty = "Hello world"; });
            #endregion

        }

        public async Task Subscribe()
        {
            IBus bus = null;
            #region ExplicitSubscribe
            await bus.SubscribeAsync<MyEvent>();

            await bus.UnsubscribeAsync<MyEvent>();
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