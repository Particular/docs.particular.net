namespace Snippets6
{
    using System.Threading.Tasks;
    using NServiceBus;

    public class BasicUsageOfIBus
    {
        async Task Send()
        {
            EndpointConfiguration configuration = new EndpointConfiguration();

            #region BasicSend
            IEndpointInstance instance = await Endpoint.Start(configuration);
            await instance.Send(new MyMessage());
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
            IEndpointInstance endpoint = null;

            #region BasicSendInterface
            await endpoint.Send<IMyMessage>(m => m.MyProperty = "Hello world");
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