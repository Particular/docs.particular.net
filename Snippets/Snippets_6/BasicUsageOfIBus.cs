namespace Snippets6
{
    using System.Threading.Tasks;
    using NServiceBus;

    public class BasicUsageOfIBus
    {
        async Task Send()
        {
            var busConfig = new BusConfiguration();

            #region BasicSend
            IEndpointInstance instance = await Endpoint.Start(busConfig);
            IBusContext busContext = instance.CreateBusContext();

            await busContext.Send(new MyMessage());
            #endregion
        }

        #region SendFromHandler

        public class MyMessageHandler : IHandleMessages<MyMessage>
        {
            public async Task Handle(MyMessage message, IMessageHandlerContext context)
            {
                await context.Send(new OtherMessage());
            }
        }
        #endregion

        async Task SendInterface()
        {
            IBusContext busContext = null;

            #region BasicSendInterface
            await busContext.Send<IMyMessage>(m => m.MyProperty = "Hello world");
            #endregion
        }

        public class MyMessage
        {
        }

        public class OtherMessage
        {
             
        }

        interface IMyMessage
        {
            string MyProperty { get; set; }
        }
    }
}