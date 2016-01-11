namespace Snippets6.Routing
{
    using System.Threading.Tasks;
    using NServiceBus;

    public class BasicOperations
    {
        public async Task ConcreteMessage()
        {
            IBusContext busContext = null;
            #region InstancePublish
            MyEvent message = new MyEvent { SomeProperty = "Hello world" };
            await busContext.Publish(message);
            #endregion

        }

        public async Task InterfaceMessage()
        {
            IBusContext busContext = null;
            #region InterfacePublish
            await busContext.Publish<IMyEvent>(m => { m.SomeProperty = "Hello world"; });
            #endregion

        }

        public async Task Subscribe()
        {
            IBusContext busContext = null;
            #region ExplicitSubscribe
            await busContext.Subscribe<MyEvent>();

            await busContext.Unsubscribe<MyEvent>();
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