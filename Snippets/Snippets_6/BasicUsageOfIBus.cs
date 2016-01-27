namespace Snippets6
{
    using System.Threading.Tasks;
    using NServiceBus;

    public class BasicUsageOfIBus
    {
        async Task Send()
        {
            BusConfiguration busConfiguration = new BusConfiguration();

            #region BasicSend
            IEndpointInstance instance = await Endpoint.Start(busConfiguration);
            
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
            IEndpointInstance endpointInstance = null;

            #region BasicSendInterface
            await endpointInstance.Send<IMyMessage>(m => m.MyProperty = "Hello world");
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