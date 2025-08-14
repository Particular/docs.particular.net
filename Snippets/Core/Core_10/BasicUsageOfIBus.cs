namespace Core;

using System.Threading.Tasks;
using NServiceBus;

class BasicUsageOfIBus
{
    async Task Send(EndpointConfiguration endpointConfiguration)
    {
        #region BasicSend

        var endpointInstance = await Endpoint.Start(endpointConfiguration);
        var message = new MyMessage();
        await endpointInstance.Send(message);

        #endregion
    }

    #region SendFromHandler

    public class MyMessageHandler :
        IHandleMessages<MyMessage>
    {
        public Task Handle(MyMessage message, IMessageHandlerContext context)
        {
            var otherMessage = new OtherMessage();
            return context.Send(otherMessage);
        }
    }

    #endregion

    async Task SetDestination(IEndpointInstance endpoint)
    {
        #region BasicSendSetDestination

        var options = new SendOptions();
        options.SetDestination("MyDestination");
        await endpoint.Send(new MyMessage(), options);

        #endregion
    }

    async Task SpecificInstance(IEndpointInstance endpoint)
    {
        #region BasicSendSpecificInstance

        var options = new SendOptions();
        options.RouteToSpecificInstance("MyInstance");
        var message = new MyMessage();
        await endpoint.Send(message, options);

        #endregion
    }

    async Task ThisEndpoint(IEndpointInstance endpoint)
    {
        #region BasicSendToAnyInstance

        var options = new SendOptions();
        options.RouteToThisEndpoint();
        await endpoint.Send(new MyMessage(), options);
        // or
        await endpoint.SendLocal(new MyMessage());

        #endregion
    }

    async Task ThisInstance(IEndpointInstance endpoint)
    {
        #region BasicSendToThisInstance

        var options = new SendOptions();
        options.RouteToThisInstance();
        var message = new MyMessage();
        await endpoint.Send(message, options);

        #endregion
    }

    async Task SendReplyToThisInstance(IEndpointInstance endpoint)
    {
        #region BasicSendReplyToThisInstance

        var options = new SendOptions();
        options.RouteReplyToThisInstance();
        var message = new MyMessage();
        await endpoint.Send(message, options);

        #endregion
    }

    async Task SendReplyToAnyInstance(IEndpointInstance endpoint)
    {
        #region BasicSendReplyToAnyInstance

        var options = new SendOptions();
        options.RouteReplyToAnyInstance();
        var message = new MyMessage();
        await endpoint.Send(message, options);

        #endregion
    }

    async Task SendReplyTo(IEndpointInstance endpoint)
    {
        #region BasicSendReplyToDestination

        var options = new SendOptions();
        options.RouteReplyTo("MyDestination");
        var message = new MyMessage();
        await endpoint.Send(message, options);

        #endregion
    }

    async Task ReplySendReplyToThisInstance(IMessageHandlerContext context)
    {
        #region BasicReplyReplyToThisInstance

        var options = new ReplyOptions();
        options.RouteReplyToThisInstance();
        var myMessage = new MyMessage();
        await context.Reply(myMessage, options);

        #endregion
    }

    async Task ReplySendReplyToAnyInstance(IMessageHandlerContext context)
    {
        #region BasicReplyReplyToAnyInstance

        var options = new ReplyOptions();
        options.RouteReplyToAnyInstance();
        var myMessage = new MyMessage();
        await context.Reply(myMessage, options);

        #endregion
    }

    async Task ReplySendReplyTo(IMessageHandlerContext context)
    {
        #region BasicReplyReplyToDestination

        var options = new ReplyOptions();
        options.RouteReplyTo("MyDestination");
        var myMessage = new MyMessage();
        await context.Reply(myMessage, options);

        #endregion
    }

    public class MyMessage
    {
    }

    public class OtherMessage
    {
    }
}