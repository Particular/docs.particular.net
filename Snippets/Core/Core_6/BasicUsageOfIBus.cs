namespace Core6
{
    using System.Threading.Tasks;
    using NServiceBus;

    class BasicUsageOfIBus
    {
        async Task Send(EndpointConfiguration endpointConfiguration)
        {
            #region BasicSend
            IEndpointInstance instance = await Endpoint.Start(endpointConfiguration);
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

        async Task SendInterface(IEndpointInstance endpoint)
        {
            #region BasicSendInterface
            await endpoint.Send<IMyMessage>(m => m.MyProperty = "Hello world");
            #endregion
        }

        async Task SetDestination(IEndpointInstance endpoint)
        {
            #region BasicSendSetDestination
            SendOptions options = new SendOptions();
            options.SetDestination("MyDestination");
            await endpoint.Send(new MyMessage(), options);
            //or
            await endpoint.Send<MyMessage>("MyDestination", m => { });
            #endregion
        }

        async Task SpecificInstance(IEndpointInstance endpoint)
        {
            #region BasicSendSpecificInstance
            SendOptions options = new SendOptions();
            options.RouteToSpecificInstance("MyInstance");
            await endpoint.Send(new MyMessage(), options);
            #endregion
        }

        async Task ThisEndpoint(IEndpointInstance endpoint)
        {
            #region BasicSendToAnyInstance
            SendOptions options = new SendOptions();
            options.RouteToThisEndpoint();
            await endpoint.Send(new MyMessage(), options);
            //or
            await endpoint.SendLocal(new MyMessage());
            #endregion
        }

        async Task ThisInstance(IEndpointInstance endpoint)
        {
            #region BasicSendToThisInstance
            SendOptions options = new SendOptions();
            options.RouteToThisInstance();
            await endpoint.Send(new MyMessage(), options);
            #endregion
        }

        async Task SendReplyToThisInstance(IEndpointInstance endpoint)
        {
            #region BasicSendReplyToThisInstance
            SendOptions options = new SendOptions();
            options.RouteReplyToThisInstance();
            await endpoint.Send(new MyMessage(), options);
            #endregion
        }

        async Task SendReplyToAnyInstance(IEndpointInstance endpoint)
        {
            #region BasicSendReplyToAnyInstance
            SendOptions options = new SendOptions();
            options.RouteReplyToAnyInstance();
            await endpoint.Send(new MyMessage(), options);
            #endregion
        }

        async Task SendReplyTo(IEndpointInstance endpoint)
        {
            #region BasicSendReplyToDestination
            SendOptions options = new SendOptions();
            options.RouteReplyTo("MyDestination");
            await endpoint.Send(new MyMessage(), options);
            #endregion
        }

        async Task ReplySendReplyToThisInstance(IMessageHandlerContext context)
        {
            #region BasicReplyReplyToThisInstance
            ReplyOptions options = new ReplyOptions();
            options.RouteReplyToThisInstance();
            await context.Reply(new MyMessage(), options);
            #endregion
        }

        async Task ReplySendReplyToAnyInstance(IMessageHandlerContext context)
        {
            #region BasicReplyReplyToAnyInstance
            ReplyOptions options = new ReplyOptions();
            options.RouteReplyToAnyInstance();
            await context.Reply(new MyMessage(), options);
            #endregion
        }

        async Task ReplySendReplyTo(IMessageHandlerContext context)
        {
            #region BasicReplyReplyToDestination
            ReplyOptions options = new ReplyOptions();
            options.RouteReplyTo("MyDestination");
            await context.Reply(new MyMessage(), options);
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